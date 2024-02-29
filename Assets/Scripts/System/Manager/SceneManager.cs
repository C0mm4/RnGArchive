using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController
{
    Image fadeOutImg;
    private float fadeOutTime = 1f;

    public async Task SceneLoad(string scene)
    {
        CreateFadeOutObj();
        await FadeOut(scene);

        GameManager.CharactorSpawnStartGame();

        await FadeIn();
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

    private async Task FadeOut(string scene)
    {
        float t = 0;
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
    }

    private async Task FadeIn()
    {
        float t = 0;
        Color color = fadeOutImg.color;
        while(t < fadeOutTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            color.a = alpha;
            fadeOutImg.color = color;

            await Task.Yield();
        }


    }

}
