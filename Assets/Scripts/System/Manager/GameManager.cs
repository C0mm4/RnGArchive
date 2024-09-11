using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

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

    TriggerManager _triggerManager = new TriggerManager();
    public static TriggerManager Trigger { get { return gm_Instance._triggerManager; } }

    GameSavemanager _gameSaveManager = new GameSavemanager();
    public static GameSavemanager Save { get { return gm_Instance._gameSaveManager; } }

    SceneController _sceneManager = new SceneController();
    public static SceneController Scene { get { return gm_Instance._sceneManager; } }

    public static SettingManager.SerializeGameData gameData;

    public static Vector2 tileOffset = new Vector2(0.32f, 0.32f);

    public static bool _isPaused;
    public static bool isPaused { get { return  _isPaused; } set { _isPaused = value; } }

    public static GameObject player;
    public static GameObject Player { get { return player; } }

    public static GameProgress gameProgress;
    public static GameProgress Progress { get { return gameProgress; } set { gameProgress = value; } }


    public static CameraManager _cameraManager;
    public static CameraManager CameraManager { get { return _cameraManager; } set { _cameraManager = value; } }


    [SerializeField]
    public Transform test;


    public GameObject testDoor;
    public KeySetting keysetting;

    public static UIState uiState;

    public GameObject currentMapObj;

    public UIManager uimanager;

    public bool pause;

    public UIState gamestate;
    public List<UIState> uiStates;

    public CharactorController characon;
    public StageController stage;

    string screenshotFolder = "Screenshots";

    public SettingManager.SerializeGameData setting;

    private void Awake()
    {
        // Set Dont Destroy Object
        DontDestroyOnLoad(gameObject);

        // Set Static Instance
        gm_Instance = this;

        // 60 FPS
        Application.targetFrameRate = 60;

        Time.timeScale = 1.0f;

        gameData = new SettingManager.SerializeGameData();

        _settingManager.SetResolutionList();
        _settingManager.SetLanguageList();

        // If Not First Run
        if (PlayerPrefs.HasKey("FirstRun"))
        {
            _settingManager.LoadSettingData();
            Debug.Log("SecondRun Seting");
        }
        // If First Run
        else
        {
            _settingManager.SetFirstSetting();
            _settingManager.SaveSettingData();
            _settingManager.LoadSettingData();
            Debug.Log("FirstFun Setting");
        }

        _settingManager.SetResolution();

        DataInit();
        isPaused = false;
        FindPlayer();

        characon = CharaCon;
        stage= Stage;
        setting = gameData;
    }


    public void DataInit()
    {
        // UI 연동은 나중에 추가 (로딩 텍스트)
        UIManager.initialize();
        FSM.init();
        Input.Initialize();
        Script.init();
        CharaCon.initialize();
        MobSpawner.Initialize();
        Save.initialize();
        Trigger.Initialize();

        Task.Run(() =>
        {

        });

        // Test Area
/*
        Stage.SetInitializeParty();*/

    }

    public void Update()
    {
        gamestate = uiState;
        uiStates = UIManager.uistack.ToList();
        FindPlayer();
        ManagerUpdate();
        if (CameraManager == null)
        {
            CameraManager cm = GameObject.Find("Main Camera").GetComponent<CameraManager>();
            CameraManager = cm;
        }

        uimanager = UIManager;
        pause = isPaused;
        setting = gameData;

        if (UnityEngine.Input.GetKeyDown(KeyCode.K))
        {
            InstantiateAsync("PartyControlUI");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.F12))
        {
            ScreenshotCapture();
        }
    }

    private void FindPlayer()
    {
        GameObject go = GameObject.Find("Player");
        if (go != null)
        {
            player = go;
        }
    }

    public static void ChangeUIState(UIState newState)
    {
        uiState = newState;

        switch (uiState)
        {
            case UIState.InPlay:
                if(Progress != null)
                {
                    if(UIManager.inGameUI != null)
                    {
                        UIManager.inGameUI.EnableUI();
                    }
                }
                break;
            default:
                if(UIManager.inGameUI != null)
                {
                    UIManager.inGameUI.DisableUI();
                }
                break;
        }
    }

    public static UIState GetUIState()
    {
        return uiState;
    }

    public static void ESCPause()
    {
        if (isPaused)
        {
//            ResumeGame();
        }
        else
        {
            InstantiateAsync("PauseUI");
        }
    }

    public static void PauseGame()
    {
        isPaused = true;
    }

    public static void ResumeGame()
    {
        isPaused = false;
    }

    public static void CharactorChange(int index)
    {
        player.GetComponent<PlayerController>().controlEnabled = false;
        CharactorSpawn(player.transform, Progress.currentParty[index].charaData.id);
        player.GetComponent<PlayerController>().canMove = true;
        UIManager.inGameUI.CharactorChange(0, index);
    }

    public void ManagerUpdate()
    {
        Input.Update();
        UIManager.Update();
        Stage.Update();
        CharaCon.Update();
    }

    public static void CharactorSpawnStartGame()
    {
        Transform pos = GameObject.Find("SpawnPoint").GetComponent<Transform>();
        CharactorSpawn(pos, Progress.currentCharactorId);
    }

    public static void CharactorSpawnInLoadGame()
    {
        GameObject go = new GameObject();
        go.transform.position = Progress.saveP;
        CharactorSpawn(go.transform, Progress.currentCharactorId);
        Destroy(go);
    }

    public static void CharactorSpawnInLoad(string doorId)
    {
        Door go = FindObjectsOfType<GameObject>().FirstOrDefault(go => go.GetComponent<Door>() != null && go.GetComponent<Door>().id == doorId).GetComponent<Door>();
        Transform pos = go.transform;
        CharactorSpawn(pos, Progress.currentCharactorId);
        player.GetComponent<PlayerController>().canMove = true;
    }

    public static void CharactorSpawn(Transform transform, string id)
    {
        Vector3 tmp = transform.position;

        var awaitObj = InstantiateAsync("Player");
        player = awaitObj;
        player.transform.position = tmp;
        player.GetComponent<PlayerController>().controlEnabled = true;
        player.GetComponent<PlayerController>().charactor = gameProgress.charaDatas[id].charactor;
        player.GetComponent<PlayerController>().CreateHandler();


        if(Progress != null)
            Progress.currentCharactorId = id;

        CameraManager.player = player.transform;
    }

    public static PlayerController CutSceneCharactorSpawn(Transform trans, string id)
    {
        GameObject go;
        go = InstantiateAsync("Player", trans.position, trans.rotation);
        go.GetComponent<PlayerController>().charactor = gameProgress.charaDatas[id].charactor;
        go.GetComponent<PlayerController>().CreateHandler();
        go.GetComponent<PlayerController>().controlEnabled = false;

        return go.GetComponent<PlayerController>();
    }

    public static GameObject InstantiateAsync(string path, Vector3 pos = default, Quaternion rotation = default)
    {
        return Resource.InstantiateAsync(path, pos, rotation);
    }


    public static T LoadAssetDataAsync<T>(string path) where T : UnityEngine.Object
    {
        return (T)Resource.LoadAssetAsync<T>(path).Result;
    }

    public static T LoadAssetDataAsync<T>(AssetReference assetReference) where T : UnityEngine.Object
    {
        return (T)Resource.LoadAssetAsync<T>(assetReference).Result;
    }

    public static void Destroy(GameObject[] gos)
    {
        Resource.Destroy(gos);
    }

    public static Sprite LoadSprite(string path)
    {
        return Resource.LoadSprite(path);
    }

    public static void Destroy(GameObject go)
    {
        Resource.Destroy(go);
    }

    public static async Task SceneControlStart(string targetScene)
    {
        if (GetUIState() != UIState.Loading)
        {
            ChangeUIState(UIState.Loading);
            await Scene.StartGame();
            ChangeUIState(UIState.InPlay);
            Debug.Log(GetUIState().ToString());
        }
    }

    public static async Task SceneControlLoad(string targetScene)
    {
        if (GetUIState() != UIState.Loading)
        {
            ChangeUIState(UIState.Loading);
            await Scene.LoadGame();
            ChangeUIState(UIState.InPlay);
            Debug.Log(GetUIState().ToString());
        }
    }

    public static async void GameStart()
    {
        Save.NewGame();
        await SceneControlStart("InGameScene");
    }

    public static async void LoadGame()
    {
        Save.LoadProgress();
        await SceneControlLoad("InGameScene");
    }

    public static async void GameEnd()
    {
        await Scene.FadeOut();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public static void SettingButton()
    {
        InstantiateAsync("SettingUI");
    }

    public static GameObject ParticleGen(string target, Vector3 startPos, Vector3 endPos, float time = 1f)
    {
        GameObject particle = InstantiateAsync(target, startPos);
        particle.GetComponent<Particle>().CreateHandler(startPos, endPos, time);
        return particle;
    }

    public static async void PlayerDie()
    {
        ChangeUIState(UIState.PlayerDie);
        ParticleGen("Particle_PlayerDie", player.transform.position, player.transform.position + new Vector3(0, 10), 3);
        ParticleGen("Particle_PlayerDie", player.transform.position, player.transform.position + new Vector3(0, -10), 3);
        ParticleGen("Particle_PlayerDie", player.transform.position, player.transform.position + new Vector3(10, 0), 3);
        ParticleGen("Particle_PlayerDie", player.transform.position, player.transform.position + new Vector3(-10, 0), 3);
        Destroy(Player);
        await Task.Delay(TimeSpan.FromSeconds(3f));
        LoadGame();
    }

    public void ScreenshotCapture()
    {
        string folderPath = Path.Combine(Application.dataPath, screenshotFolder);

        if(!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        string screenshotName = "Screenshot_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        string filepath = Path.Combine(folderPath, screenshotName);
        ScreenCapture.CaptureScreenshot(filepath);
    }
}
