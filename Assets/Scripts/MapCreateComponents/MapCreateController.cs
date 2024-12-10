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

public class MapCreateController : MonoBehaviour
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

    public string currentMapName;

    public enum InputMode
    {
        draw, erase, selectObj, selectTrigger, drawCutSceneTrigger,  drawSpawnTrigger, drawSpawnPoint,
    }
    [SerializeField]
    public InputMode inputMode = InputMode.draw;

    public MapCreateInspector inspector;

    public GameObject triggerInspectorPrefab;
    public GameObject spawntriggerInspectorPrefab;
    public GameObject spawnPointInspectorPrefab;
    public GameObject doorInspectorPrefab;
    public GameObject mapInspectorPrefab;

    public GameObject invisableObjectsParent;

    public CursorUI cursorUI;

    public bool isRename;
    public string newName;
    public bool forceShow;

    public Material LineMaterial;

    // Start is called before the first frame update
    public void Start()
    {
        LoadMapPrefab();
        LoadTileDropdown();
        SetCameraOrigin();
        animator = GetComponent<Animator>();

        GameManager.isPaused = true;

        mapSelect.value = 0;
        tileSelect.value = 0;
        selectLayer.value = 0;

        CreateNewMap();
        cursorUI.SetButton(0);
    }

    // Update is called once per frame
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (!isRename)
            {
                isRename = true;
                newName = "";
            }
            else
            {
                mapSelect.captionText.text = currentMapName;
                isRename = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            forceShow = !forceShow;
        }

        if(isRename)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                newName = "";
                mapSelect.captionText.text = currentMapName;
                isRename = false;
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {

                currentMap.name = newName;
                mapSelect.captionText.text = newName;
                newName = "";
                isRename = false;
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if(newName.Length > 0)
                {
                    newName = newName.Substring(0, newName.Length - 1);
                    mapSelect.captionText.text = newName;
                }
            }

            foreach (char c in Input.inputString)
            {
                // 입력된 키가 특수 키가 아닌 경우에만 추가
                if (!char.IsControl(c))
                {
                    newName += c;

                    mapSelect.captionText.text = newName;
                }
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (inputMode == InputMode.selectObj)
                {
                    inputMode = InputMode.selectTrigger;

                    cursorUI.SetButton(3);
                }
                else
                {
                    inputMode = InputMode.selectObj;

                    cursorUI.SetButton(2);
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                inputMode = InputMode.draw;

                cursorUI.SetButton(0);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                inputMode = InputMode.erase;

                cursorUI.SetButton(1);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (inputMode == InputMode.drawCutSceneTrigger)
                {
                    inputMode = InputMode.drawSpawnTrigger;

                    cursorUI.SetButton(5);
                }
                else
                {
                    inputMode = InputMode.drawCutSceneTrigger;

                    cursorUI.SetButton(4);
                }
            }
        }

        if (isMouseInInspector() || isRename || forceShow)
        {
            if (!isShow && !isAnimationPlay)
            {
                isAnimationPlay = true;
                animator.Play("Show");
                isShow = true;
            }
        }
        else
        {
            if (isShow && !isAnimationPlay)
            {
                isAnimationPlay = true;
                animator.Play("Hide");
                isShow = false;
            }
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
        if (prefab == null)
        {
            Debug.Log("ASDF");
            return false;
        }
        var obj = Instantiate(prefab);
        if(obj != null)
        {
            currentMap = obj.GetComponent<Map>();
            obj.name = mapSelect.options[mapSelect.value].text;
            GameManager.CameraManager.background = currentMap.bound;
            currentMapName = name;
            SetCameraOrigin();
            SetLayerData();

            SetObjectLines();

            OnLayerDropDownChnage();


            SetInspector(null);
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
            currentMap.SaveBeforeStep($"Assets/Resources/Maps/{obj.name}.prefab");
            if (PrefabUtility.SaveAsPrefabAsset(obj, $"Assets/Resources/Maps/{obj.name}.prefab") != null)
            {
                GameManager.UIManager.SetText($"{obj.name} map save successfully");
                GameObject savedPrefab = Resources.Load<GameObject>($"Maps/{obj.name}");
                if (!currentMapName.Equals(obj.name) && !currentMapName.Equals("MapTemplate"))
                {
                    if (AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Resources/Maps/{currentMapName}.prefab"))
                    {
                        AssetDatabase.DeleteAsset($"Assets/Resources/Maps/{currentMapName}.prefab");
                    }
                }
                mapSelect.options[mapSelect.value].text = obj.name;
                mapSelect.captionText.text = obj.name;
                currentMapName = obj.name;

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

        foreach (var obj in data)
        {
            // 타일이 GameObject일 경우에 대한 처리
            if (obj is Tile tile)
            {
                var tileButton = Instantiate(tileButtonPrefab, tileView.transform);
                if (tileButton != null)
                {
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
            if(obj is RuleTile ruletile)
            {
                Debug.Log(ruletile.name);
                var tileButton = Instantiate(tileButtonPrefab, tileView.transform);
                if (tileButton != null)
                {
                    tileButton.GetComponent<TileButton>().controller = this;
                    tileButton.GetComponent<TileButton>().img.sprite = ruletile.m_DefaultSprite;
                    tileButton.GetComponent<TileButton>().tile = ruletile;
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

    public void AnimationEnd()
    {
        isAnimationPlay = false;
    }

    public void SetInspector(GameObject go)
    {
        if (inspector != null)
        {
            DeleteInspector();
        }
        if (go == null)
        {
            // click object is null, set map inspector
            inspector = Instantiate(mapInspectorPrefab).GetComponent<MapInspector>();
            inspector.controller = this;
            inspector.gameObject.transform.SetParent(transform);

            inspector.GetComponent<RectTransform>().localPosition = new Vector3(0, -260, 0);

            inspector.SetData(go);
        }
        else
        {
            DataShowObj = go;
            // if Object is Trigger
            var trig = go.GetComponent<Trigger>();
            if (trig != null)
            {
                // if Object is Cut Scene Trigger, set cut scene trigger inspector
                if (trig.type == Trigger.TriggerType.CutScene)
                {

                    inspector = Instantiate(triggerInspectorPrefab).GetComponent<MapCreateInspector>();
                    inspector.controller = this;
                    inspector.gameObject.transform.SetParent(transform);

                    inspector.GetComponent<RectTransform>().localPosition = new Vector3(0, -260, 0);

                    inspector.SetData(go);
                }
                // if object is spawn trigger inspector, set spawn trigger inspector
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
                // if object is door, set door inspector
                var door = go.GetComponent<Door>();
                if (door != null)
                {
                    inspector = Instantiate(doorInspectorPrefab).GetComponent<DoorInspector>();
                    inspector.controller = this;
                    inspector.gameObject.transform.SetParent(transform);

                    inspector.GetComponent<RectTransform>().localPosition = new Vector3(0, -260, 0);

                    inspector.SetData(go);
                }
                // if object is spawn Point, set spawn point inspector
                else
                {                
                    var spawnP = go.GetComponent<SpawnP>();
                    if (spawnP != null)
                    {
                        inspector = Instantiate(spawnPointInspectorPrefab).GetComponent<SpawnPointInspector>();
                        inspector.controller = this;
                        inspector.gameObject.transform.SetParent(transform);

                        inspector.GetComponent<RectTransform>().localPosition = new Vector3(0, -260, 0);

                        inspector.SetData(go);
                    }
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
//            go.GetComponent<Trigger>().drawLine(Color.red);
            DrawTriggerLine(Color.red, go.GetComponent<Trigger>());
            if(go.GetComponent<Trigger>().type == Trigger.TriggerType.Spawn)
            {
                foreach(Transform spawnP in trig.transform)
                {
                    DrawSpawnPoint(Color.cyan, spawnP);
                }
            }
        }
        foreach(Transform spawnP in currentMap.SpawnPointParents.transform)
        {
            DrawSpawnPoint(Color.green, spawnP);
        }
    }


    public void DrawTriggerLine(Color color, Trigger trig)
    {
        LineRenderer line = trig.GetComponent<LineRenderer>();
        if (line == null)
            line = trig.AddComponent<LineRenderer>();
        line.material = LineMaterial;

        line.loop = true;
        Vector3[] positions = new Vector3[5];

        positions[0] = trig.transform.position + new Vector3(-trig.GetComponent<BoxCollider2D>().size.x / 2, trig.GetComponent<BoxCollider2D>().size.y / 2, -1);
        positions[1] = trig.transform.position + new Vector3(trig.GetComponent<BoxCollider2D>().size.x / 2, trig.GetComponent<BoxCollider2D>().size.y / 2, -1);
        positions[2] = trig.transform.position + new Vector3(trig.GetComponent<BoxCollider2D>().size.x / 2, -trig.GetComponent<BoxCollider2D>().size.y / 2, -1);
        positions[3] = trig.transform.position + new Vector3(-trig.GetComponent<BoxCollider2D>().size.x / 2, -trig.GetComponent<BoxCollider2D>().size.y / 2, -1);
        positions[4] = positions[0];

        line.startColor = color;
        line.endColor = color;

        line.startWidth = 0.01f;
        line.endWidth = 0.01f;

        line.positionCount = 5;

        line.useWorldSpace = true;

        line.SetPositions(positions);

    }

    public void DrawSpawnPoint(Color color, Transform trans)
    {
        LineRenderer line = trans.GetComponent<LineRenderer>();
        if (line == null)
            line = trans.AddComponent<LineRenderer>();
        line.material = LineMaterial;

        line.loop = true;
        Vector3[] positions = new Vector3[5];

        positions[0] = trans.transform.position + new Vector3(-0.16f, 0.16f);
        positions[1] = trans.transform.position + new Vector3(0.16f, 0.16f);
        positions[2] = trans.transform.position + new Vector3(0.16f, -0.16f);
        positions[3] = trans.transform.position + new Vector3(-0.16f, -0.16f);
        positions[4] = positions[0];

        line.startColor = color;
        line.endColor = color;

        line.startWidth = 0.01f;
        line.endWidth = 0.01f;

        line.positionCount = 5;

        line.useWorldSpace = true;

        line.SetPositions(positions);

    }

    public bool isMouseInInspector()
    {
        float xMin, xMax;
        Vector3 mousePos = Input.mousePosition;

        if (isShow)
        {
            xMin = GameManager.CameraManager.maincamera.pixelWidth - 500;
        }
        else
        {
            xMin = GameManager.CameraManager.maincamera.pixelWidth - 50;
        }
        xMax = GameManager.CameraManager.maincamera.pixelWidth;


        if (mousePos.x >= xMin && mousePos.x <= xMax)
        {
            return true;
        }
        else return false;
    }
}
