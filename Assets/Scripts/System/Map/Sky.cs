using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Sky : TilemapParallax
{
    public SpriteRenderer skySprite;
    public Material skyMaterial;
    public float baseX, baseY;

    public override void OnCreate()
    {
        base.OnCreate();
        baseX = 3.04f;
        baseY = 1.7f;


    }

    public override void StepAlways()
    {
        base.StepAlways();

        
        // Set SkySprite Position, Scale
        skySprite.transform.localScale = new Vector3(
            20 * 0.32f + Mathf.Max(GetComponent<Tilemap>().cellBounds.size.x - 20, 0) * (1 - parallaxEffectX) * 0.32f,
            11 * 0.32f + Mathf.Max(GetComponent<Tilemap>().cellBounds.size.y - 11, 0) * (1 - parallaxEffectY) * 0.32f, 
            0
            );
        skySprite.transform.localPosition = new Vector3(
            baseX + Mathf.Max(GetComponent<Tilemap>().cellBounds.size.x - 20, 0) * (1 - parallaxEffectX) * 0.16f,
            baseY + Mathf.Max(GetComponent<Tilemap>().cellBounds.size.y - 11, 0) * (1 - parallaxEffectY) * 0.16f,
            0
            );


        skyMaterial.SetFloat("_CustomTime", GameManager.GameTime / 1440f);
    }
}
