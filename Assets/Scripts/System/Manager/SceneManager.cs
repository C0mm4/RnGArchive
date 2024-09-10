using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController
{
    Image fadeOutImg;
    private float fadeOutTime = 1f;


    
    public async Task StartGame()
    {
        CreateFadeOutObj();
        await FadeOut("InGameScene");

        await LoadMap("10011000");
        GameManager.CharactorSpawnStartGame();
        await Task.Delay(TimeSpan.FromSeconds(1f));

        await FadeIn();
    }

    public async Task StartGameAfterOpening()
    {
        CreateFadeOutObj();
        await FadeOut("InGameScene");

        await LoadMap("10012100");
        GameManager.CharactorSpawnStartGame();

        GameManager.Save.SaveGameprogress(GameManager.Player.transform);
        await Task.Delay(TimeSpan.FromSeconds(.5f));

        GameManager.ChangeUIState(UIState.InPlay);
    }

    public async Task LoadGame()
    {
        CreateFadeOutObj();
        await FadeOut("InGameScene");
        GameManager.Save.LoadProgress();
        await LoadMap(GameManager.Progress.saveMapId);
        GameManager.CharactorSpawnInLoadGame();

        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        Debug.Log(GameManager.Progress.activeTrigs.Count);
        if(GameManager.Progress.activeTrigs.Count != 1)
        {
            await FadeIn();
        }
    }

    public async Task MoveMap(string mapId, string doorId)
    {
        GameManager.ChangeUIState(UIState.Loading);

        CreateFadeOutObj();
        await FadeOut("InGameScene");
        GameManager.Stage.LoadMap(mapId);
        GameManager.CharactorSpawnInLoad(doorId);
        await FadeIn();
        GameManager.ChangeUIState(UIState.InPlay);
    }

    public async Task LoadMap(string mapId)
    {
        GameManager.ChangeUIState(UIState.Loading);
        CreateFadeOutObj();
        await FadeOut("InGameScene");
        GameManager.Stage.LoadMap(mapId);
    }

    public void CreateFadeOutObj()
    {
        Canvas canvas = GameManager.UIManager.canvas.GetComponent<Canvas>();

        if(canvas != null)
        {
            GameObject fadeOut = GameManager.Resource.InstantiateAsync("FadeOut");
            fadeOut.transform.SetParent(canvas.transform, false);
            fadeOut.transform.localPosition = Vector3.zero;

            RectTransform rect = fadeOut.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;

            fadeOutImg = fadeOut.GetComponent<Image>();

        }
        else
        {
            Debug.LogError("Can't Find Canvas Object in this Scene");
        }
    }

    public void CreateBlackOutObj()
    {
        Canvas canvas = GameManager.UIManager.canvas.GetComponent<Canvas>();

        if (canvas != null)
        {
            GameObject fadeOut = GameManager.Resource.InstantiateAsync("FadeOut");
            fadeOut.transform.SetParent(canvas.transform, false);
            fadeOut.transform.localPosition = Vector3.zero;

            RectTransform rect = fadeOut.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;

            if(fadeOutImg != null)
            {
                GameManager.Destroy(fadeOutImg.gameObject);
            }

            fadeOutImg = fadeOut.GetComponent<Image>();
            fadeOutImg.color = Color.black;
        }
        else
        {
            Debug.LogError("Can't Find Canvas Object in this Scene");
        }
    }

    private async Task FadeOut(string scene)
    {
        float t = 0;
        if (fadeOutImg != null)
        {
            fadeOutImg = GameObject.Find("fadeOut 1(Clone)").GetComponent<Image>();
        }

        if(fadeOutImg != null)
        {
            CreateFadeOutObj();
        }
        Color color = fadeOutImg.color;
        while(t < fadeOutTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t/fadeOutTime);
            color.a = alpha;
            fadeOutImg.color = color;

            await Task.Yield();
        }
        var sceneLoadAsync = SceneManager.LoadSceneAsync(scene);
        while(!sceneLoadAsync.isDone)
        {
            await Task.Yield();
        }
        CreateFadeOutObj();
        fadeOutImg.color = Color.black;
    }

    public async Task FadeIn()
    {
        float t = 0;
        if(fadeOutImg != null)
        {
            fadeOutImg = GameObject.Find("fadeOut 1(Clone)").GetComponent<Image>();
        }
        Color color = fadeOutImg.color;
        while(t < fadeOutTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            color.a = alpha;
            fadeOutImg.color = color;

            await Task.Yield();
        }

        GameManager.Destroy(fadeOutImg.gameObject);
    }

    public async Task FadeOut()
    {

        float t = 0;
        if (fadeOutImg != null)
        {
            fadeOutImg = GameObject.Find("fadeOut 1(Clone)").GetComponent<Image>();
        }

        if (fadeOutImg == null)
        {
            CreateFadeOutObj();
        }
        Color color = fadeOutImg.color;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeOutTime);
            color.a = alpha;
            fadeOutImg.color = color;

            await Task.Yield();
        }
    }
}
