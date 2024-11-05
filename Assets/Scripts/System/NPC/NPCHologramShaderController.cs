using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class NPCHologramShaderController : Obj
{
    private Sprite currentSprite;
    private SpriteRenderer[] spriteRenderer;
    public Material material;
    public string address = "Assets/Materials/NPCHolo.mat";

    public async override void OnCreate()
    {
        base.OnCreate();

        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        
//        LoadMeterial();

        await Appear();
    }


    public override void BeforeStep()
    {
        base.BeforeStep();
           
        if (currentSprite != spriteRenderer[0].sprite)
        {
            currentSprite = spriteRenderer[0].sprite;
            SetUVCoordinate();
        }
    }

    public void SetUVCoordinate()
    {
        foreach (var renderer in spriteRenderer)
        {
            renderer.material = material;
            Rect textureRect = renderer.sprite.textureRect;
            Vector2 textureSize = new Vector2(renderer.sprite.texture.width, renderer.sprite.texture.height);

            Vector4 uvRange = new Vector4(
                textureRect.x / textureSize.x,
                textureRect.y / textureSize.y,
                textureRect.width / textureSize.x,
                textureRect.height / textureSize.y
                );

            renderer.material.SetVector("_UVRange", uvRange);

        }
    }

    private void LoadMeterial()
    {
        Addressables.LoadAssetAsync<Material>(address).Completed += (handle) =>
        {
            if(handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                material = handle.Result;
                SetUVCoordinate();
            }
        };
    }

    public async Task Appear()
    {
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;

            foreach (var renderer in spriteRenderer)
            {

                renderer.material.SetFloat("_DisplayPercentage", t);

            }

            await Task.Yield();
        }


        foreach (var renderer in spriteRenderer)
        {
            renderer.material.SetFloat("_DisplayPercentage", 1);

        }

    }

    public async Task Disappear()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;

            foreach (var renderer in spriteRenderer)
            {

                renderer.material.SetFloat("_DisplayPercentage", 1 - t);

            }

            await Task.Yield();
        }


        foreach (var renderer in spriteRenderer)
        {
            renderer.material.SetFloat("_DisplayPercentage", 0);

        }

        Destroy();
    }
}
