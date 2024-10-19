using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapCreateController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Map currentMap;
    public GameObject mapCenterObj;

    public TMP_Dropdown mapSelect;
    public TMP_Dropdown tileSelect;
    public TMP_Dropdown selectLayer;

    public GameObject currentTileMap;
    public GameObject tileView;

    public Tilemap currentLayer;
    public Object selectedTile;

    public GameObject tileButtonPrefab;

    public Tilemap selectedTilemap;
    private Animator animator;

    private bool isDropDownOpen = false;
    private bool isAnimationPlay = false;
    public bool isShow;

    public GameObject DataShowObj;

    public enum InputMode
    {
        draw, erase, selectObj, selectTrigger, drawCutSceneTrigger,  drawSpawnTrigger
    }
    [SerializeField]
    public InputMode inputMode = InputMode.draw;

    MapCreateInspector inspector;

    public GameObject triggerInspectorPrefab;
    public GameObject spawntriggerInspectorPrefab;
    public GameObject spawnPointInspectorPrefab;
    public GameObject doorInspectorPrefab;

    public GameObject invisableObjectsParent;
    

    // Start is called before the first frame update
    void Start()
    {
        LoadMapPrefab();
        LoadTileDropdown();
        SetCameraOrigin();
        animator = GetComponent<Animator>();

        GameManager.PauseGame();

        mapSelect.value = 0;
        tileSelect.value = 0;
        selectLayer.value = 0;

        CreateNewMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(inputMode == InputMode.selectObj)
            {
                inputMode = InputMode.selectTrigger;
            }
            else inputMode = InputMode.selectObj;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            inputMode = InputMode.draw;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            inputMode = InputMode.erase;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(inputMode == InputMode.drawCutSceneTrigger)
            {
                inputMode = InputMode.drawSpawnTrigger;
            }
            else inputMode = InputMode.drawCutSceneTrigger;
        }

        if(mapSelect.IsExpanded || selectLayer.IsExpanded || tileSelect.IsExpanded)
        {
            isDropDownOpen = true;
        }
        else
        {
            isDropDownOpen = false;
        }
    }

    public void CreateNewMap()
    {
        mapSelect.value = 0;
        selectLayer.value = 0;
        LoadMapFile("MapTemplate");
        LoadMapPrefabOnCreateNewMap();
        currentMap.gameObject.name = "new Map";
    }

    private void LoadMapPrefab()
    {
        int index = mapSelect.value;
        GameObject[] mapPrefabs = Resources.LoadAll<GameObject>("Maps");

        List<string> maps = new List<string>();

        foreach(var map in mapPrefabs)
        {
            maps.Add(map.name);
        }

        mapSelect.ClearOptions();
        mapSelect.AddOptions(maps);
//        mapSelect.value = index;
    }

    private void LoadMapPrefabOnCreateNewMap()
    {
        GameObject[] mapPrefabs = Resources.LoadAll<GameObject>("Maps");

        List<string> maps = new List<string>();

        foreach (var map in mapPrefabs)
        {
            maps.Add(map.name);
        }
        maps.Add("New Map");

        mapSelect.ClearOptions();
        mapSelect.AddOptions(maps);
        mapSelect.value = mapSelect.options.Count - 1;
    }

    public bool LoadMapFile(string name)
    {
        if (currentMap != null)
            currentMap.Destroy();
        var prefab = Resources.Load<GameObject>($"Maps/{name}");
        if (prefab == null) return false;
        var obj = Instantiate(prefab);
        if(obj != null)
        {
            currentMap = obj.GetComponent<Map>();
            obj.name = mapSelect.options[mapSelect.value].text;
            GameManager.CameraManager.background = currentMap.bound;
            SetCameraOrigin();
            SetLayerData();

            SetObjectLines();

            OnLayerDropDownChnage();
            return true;
        }
        else
            return false;
    }

    private void SetLayerData()
    {
        var layers = currentMap.GetComponentsInChildren<TilemapRenderer>();
        List<string> options = new();
        foreach (var layer in layers)
        {
            options.Add(layer.name);
            Debug.Log(layer.name);
        }
        selectLayer.ClearOptions();
        selectLayer.AddOptions(options);
    }

    public void SaveMapFile()
    {
        var obj = currentMap.gameObject;
        if(obj != null)
        {
            if (PrefabUtility.SaveAsPrefabAsset(obj, $"Assets/Resources/Maps/{obj.name}.prefab") != null)
            {
                GameManager.UIManager.SetText($"{obj.name} map save successfully");
                return;
            }
            else
            {
                GameManager.UIManager.SetText($"{obj.name} map save failed");
                return;
            }

        }

        GameManager.UIManager.SetText($"current Map is null");
        return;
    }

    private void LoadTileDropdown()
    {
        List<GameObject> tilemaps = new();
        Addressables.LoadAssetsAsync<GameObject>("TileMap", prefab => tilemaps.Add(prefab)).Completed += handle =>
        {
            if(handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                List<string> tileMapName = new();
                foreach (var tilemap in tilemaps)
                {
                    tileMapName.Add(tilemap.name);
                }
                tileSelect.ClearOptions();
                tileSelect.AddOptions(tileMapName);

                LoadTileFile(tileSelect.options[tileSelect.value].text);
            }
        };


    }

    public void LoadTileFile(string name)
    {
        if(currentTileMap != null)
        {
            GameManager.Destroy(currentTileMap);
        }
        Addressables.InstantiateAsync($"{name}").Completed += handle =>
        {
            if(handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                currentTileMap = handle.Result;
                currentTileMap.SetActive(false);
                DisplayTiles();
            }
        };
    }


    private void DisplayTiles()
    {
        List<GameObject> pastTiles = new();
        foreach(Transform child in tileView.transform)
        {
            pastTiles.Add(child.gameObject);
        }

        GameManager.Destroy(pastTiles.ToArray());

        if (currentTileMap == null)
        {
            Debug.LogError("currentTileMap is null. Ensure that it is loaded correctly.");
            return;
        }

        CustomTilemap customTilemap = currentTileMap.GetComponentInChildren<CustomTilemap>();
        if (customTilemap == null)
        {
            Debug.LogError("CustomTilemap component is missing in the currentTileMap prefab.");
            return;
        }

        List<Object> data = customTilemap.GetAllTiles();
        Debug.Log(data.Count);

        foreach (var obj in data)
        {
            // 타일이 GameObject일 경우에 대한 처리
            if (obj is Tile tile)
            {
                var tileButton = Instantiate(tileButtonPrefab, tileView.transform);
                if (tileButton != null)
                {
                    Debug.Log("TileButton instantiated successfully.");
                    tileButton.GetComponent<TileButton>().controller = this;
                    tileButton.GetComponent<TileButton>().img.sprite = tile.sprite;
                    tileButton.GetComponent<TileButton>().tile = tile;
                    // tileButton에 타일 정보를 설정하는 코드 추가
                }
                else
                {
                    Debug.LogError("Failed to instantiate TileButton prefab.");
                }
            }

            if (obj is GameObject go)
            {
                var tileButton = Instantiate(tileButtonPrefab, tileView.transform);
                if (tileButton != null)
                {
                    tileButton.GetComponent<TileButton>().controller = this;
                    tileButton.GetComponent<TileButton>().img.sprite = go.GetComponentInChildren<SpriteRenderer>().sprite;
                    tileButton.GetComponent<TileButton>().tile = go;
                }
            }
        }

    }


    public void SetCameraOrigin()
    {
        mapCenterObj.transform.position = new Vector3(3.04f, 1.68f);
    }

    public void OnMapDropDownChnage()
    {
        if (!mapSelect.options[mapSelect.value].text.Equals("New Map"))
        {
            Debug.Log(mapSelect.options[mapSelect.value].text);
            LoadMapFile(mapSelect.options[mapSelect.value].text);
//            LoadMapPrefab();
        }
    }

    public void OnLayerDropDownChnage()
    {
        var layers = currentMap.GetComponentsInChildren<Tilemap>();
        foreach (var layer in layers)
        {
            if(layer.name.Equals(selectLayer.options[selectLayer.value].text))
            {
                currentLayer = layer;
            }
        }
    }

    public void OnTilemapDropDownChange()
    {
        LoadTileFile(tileSelect.options[tileSelect.value].text);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDropDownOpen && !isAnimationPlay)
        {
            isAnimationPlay = true;
            animator.Play("Show");
            isShow = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDropDownOpen && !isAnimationPlay) 
        {
            isAnimationPlay = true;
            animator.Play("Hide");
            isShow = false;
        }
    }
    public void AnimationEnd()
    {
        isAnimationPlay = false;
    }

    public void SetInspector(GameObject go)
    {
        DataShowObj = go;
        if(inspector != null)
        {
            DeleteInspector();
        }
        var trig = go.GetComponent<Trigger>();
        if (trig != null)
        {
            if(trig.type == Trigger.TriggerType.CutScene)
            {

                inspector = Instantiate(triggerInspectorPrefab).GetComponent<MapCreateInspector>();
                inspector.controller = this;
                inspector.gameObject.transform.SetParent(transform);

                inspector.GetComponent<RectTransform>().localPosition = new Vector3(0, -260, 0);

                inspector.SetData(go);
            }
            else
            {
                inspector = Instantiate(spawntriggerInspectorPrefab).GetComponent<MapCreateInspector>();
                inspector.controller = this;
                inspector.gameObject.transform.SetParent(transform);

                inspector.GetComponent<RectTransform>().localPosition = new Vector3(0, -260, 0);

                inspector.SetData(go);
            }
        }
        else
        {
            var spawnP = go.GetComponent<SpawnP>();
            if(spawnP != null)
            {
                inspector = Instantiate(spawnPointInspectorPrefab).GetComponent<SpawnPointInspector>();
                inspector.controller = this;
                inspector.gameObject.transform.SetParent(transform);

                inspector.GetComponent<RectTransform>().localPosition = new Vector3(0, -260, 0);

                inspector.SetData(go);
            }
            else
            {
                var door = go.GetComponent<Door>();
                if(door != null)
                {
                    inspector = Instantiate(doorInspectorPrefab).GetComponent<DoorInspector>();
                    inspector.controller = this;
                    inspector.gameObject.transform.SetParent(transform);

                    inspector.GetComponent<RectTransform>().localPosition = new Vector3(0, -260, 0);

                    inspector.SetData(go);
                }
            }
        }
    }

    public void DeleteInspector()
    {
        GameManager.Resource.Destroy(inspector.gameObject);
    }

    public void SetObjectLines()
    {
        foreach(Transform trig in currentMap.TriggerParent.transform)
        {
            GameObject go = trig.gameObject;
            go.GetComponent<Trigger>().drawLine(Color.red);
        }
    }
}
