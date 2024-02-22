using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    static GameManager gm_Instance;
    public static GameManager Instance
    {
        get
        {
            // if instance is NULL create instance
            if (!gm_Instance)
            {
                gm_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (gm_Instance == null)
                    Debug.Log("instance is NULL_GameManager");
            }
            return gm_Instance;
        }
    }


    ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return gm_Instance._resource; } }

    FSMManager _fsmManager = new FSMManager();
    public static FSMManager FSM { get { return gm_Instance._fsmManager; } }

    ScriptManager _scriptManager = new ScriptManager();
    public static ScriptManager Script { get { return gm_Instance._scriptManager; } }

    UIManager _uiManager = new UIManager();
    public static UIManager UIManager { get { return gm_Instance._uiManager; } }

    InputManager _inputManager = new InputManager();
    public static InputManager Input { get { return gm_Instance._inputManager; } }

    SettingManager _settingManager = new SettingManager();
    public static SettingManager Setting { get { return gm_Instance._settingManager; } }

    StageController _stageController = new StageController();
    public static StageController Stage { get { return gm_Instance._stageController; } }

    CharactorController _charactorController = new CharactorController();
    public static CharactorController CharaCon { get { return gm_Instance._charactorController; } }

    MobSpawner _mobSpawner = new MobSpawner();
    public static MobSpawner MobSpawner { get { return gm_Instance._mobSpawner; } }


    public static SettingManager.SerializeGameData gameData;

    public static Vector2 tileOffset = new Vector2(0.32f, 0.32f);

    public static bool isPaused;

    public static GameObject player;
    public static GameObject Player { get { return player; } }



    public static CameraManager camera;
    public static CameraManager Camera { get { return camera; } }

    [SerializeField]
    public Transform test;

    public GameObject testObj;
    public AsyncOperationHandle<GameObject> handle;


    private void Awake()
    {
        // Set Dont Destroy Object
        DontDestroyOnLoad(gameObject);

        // Set Static Instance
        gm_Instance = this;

        // 60 FPS
        Application.targetFrameRate = 60;

        Time.timeScale = 1.0f;


        _settingManager.SetResolutionList();
        _settingManager.SetLanguageList();

        // If Not First Run
        if (PlayerPrefs.HasKey("FirstRun"))
        {
            gameData = _settingManager.LoadSettingData();

        }
        // If First Run
        else
        {
            gameData = _settingManager.SetFirstSetting();
            _settingManager.SaveSettingData();
            gameData = _settingManager.LoadSettingData();

        }

        _settingManager.SetResolution();

        DataInit();
        isPaused = false;
        FindPlayer();
        camera = GameObject.Find("Main Camera").GetComponent<CameraManager>();
    }


    public void DataInit()
    {
        Debug.Log("Start Initialize");
        // UI 연동은 나중에 추가 (로딩 텍스트)
        UIManager.initialize();
        Debug.Log("UIManager Initialize");
        FSM.init();
        Debug.Log("FSM Manager Initialize");
        Script.init();
        Debug.Log("Script Manager Initialize");
        CharaCon.initialize();
        Debug.Log("Charactor Data Initialize");
        Input.Initialize();
        Debug.Log("Input Manager Initialize");
        MobSpawner.Initialize();

        Task.Run(() =>
        {

        });

        // Test Area

        Stage.SetInitializeParty();
        
    }

    public void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(Setting.gameData.masterVolume);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.G))
        {
            Setting.SettingChange("masterVolume", 1f);
        }

        FindPlayer();
        ManagerUpdate();
        if(camera == null)
        {
            camera = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
        {
            CharactorSpawn(test, 10001001);
        }

        MobSpawner.Step();

        if (UnityEngine.Input.GetKeyDown(KeyCode.X))
        {
            testObj = InstantiateAsync("test", Vector3.zero, new Quaternion());
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.C))
        {
            Addressables.ReleaseInstance(handle);
        }

    }

    private void FindPlayer()
    {
        GameObject go = GameObject.Find("Player");
        if(go != null)
        {
            player = go;
        }
    }

    public static UIManager.UIState GetUIState()
    {
        return UIManager.GetUIState();
    }

    public static void ESCPause()
    {
        PauseGame();
    }

    public static void PauseGame()
    {
        isPaused = true;
    }

    public static void ResumeGame()
    {
        isPaused = false;
    }

    public static void NextCharactor()
    {
        player.GetComponent<PlayerController>().controlEnabled = false;
        CharactorSpawn(player.transform, Stage.party[Stage.currentIndex].status.id);
    }

    public void ManagerUpdate()
    {
        Input.Update();
        UIManager.Update();
        Stage.Update();
    }

    public static void CharactorSpawn(Transform transform, int id)
    {
        Vector3 tmp = transform.position;

        player = InstantiateAsync("Player");
        player.transform.position = tmp;
        player.GetComponent<PlayerController>().controlEnabled = true;
        player.GetComponent<PlayerController>().charactor = CharaCon.charactors[id];
        player.GetComponent<PlayerController>().CreateHandler();

        
    }

    public static GameObject InstantiateAsync(string path, Vector3 pos = default, Quaternion rotation = default)
    {
        return Resource.InstantiateAsync(path, pos, rotation);
    }

    public static void Destroy(GameObject[] gos)
    {
        Resource.Destroy(gos);
    }

    public static void Destroy(GameObject go)
    {
        Resource.Destroy(go);
    }
}
