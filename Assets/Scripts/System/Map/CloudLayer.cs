using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CloudLayer : TilemapParallax
{
    public List<GameObject> clouds;
    public float cloudSpd;
    public float tilemapRightMax;
    float minY, maxY;

    public GameObject cloudPrefab;

    public Material cloudMaterial;

    float lastCloudT;
    float nextT;

    public List<Sprite> cloudSprites;

    public override void OnCreate()
    {
        base.OnCreate();
        tilemapRightMax = 20 * 0.32f + Mathf.Max(GetComponent<Tilemap>().cellBounds.size.x - 20, 0) * (1 - parallaxEffectX) * 0.32f;
        maxY = GetComponent<Tilemap>().cellBounds.size.y * 0.32f;
        minY = maxY / 2;
        nextT = 0;
        CloudInitialize();
    }

    public override void StepAlways()
    {
        base.StepAlways();
        tilemapRightMax = 20 * 0.32f + Mathf.Max(GetComponent<Tilemap>().cellBounds.size.x - 20, 0) * (1 - parallaxEffectX) * 0.32f;

        moveClouds();

        lastCloudT += Time.deltaTime;

        if(lastCloudT > nextT)
        {
            CreateCloud();
            lastCloudT = 0;
            nextT = Random.Range(1.5f, 2);
        }

        DeleteCloud();

        cloudMaterial.SetFloat("_CustomTime", GameManager.GameTime / 1440f);
    }

    public void moveClouds()
    {
        foreach (var cloud in clouds)
        {
            cloud.transform.position -= new Vector3(cloudSpd, 0, 0) * Time.deltaTime;
        }
    }

    public void CloudInitialize()
    {
        float xMin, xMax;
        xMin = 0;
        xMax = tilemapRightMax + 0.64f;

        for(int i = 0; i < GetComponent<Tilemap>().size.x; i += 2)
        {
            float randomX = Random.Range(xMin, xMax);
            float randomY = Random.Range(minY, maxY);

            int randomIndex = Random.Range(0, 21);


            GameObject go = Instantiate(cloudPrefab, new Vector3(randomX, randomY, 9), Quaternion.identity, transform);
            go.GetComponent<SpriteRenderer>().sprite = cloudSprites[randomIndex];
            clouds.Add(go);
        }

    }

    public void CreateCloud()
    {
        float newCloudX = tilemapRightMax + 0.64f;
        float randomY = Random.Range(minY, maxY);

        int randomIndex = Random.Range(0, 21);

        GameObject go = Instantiate(cloudPrefab, new Vector3(newCloudX, randomY, 9), Quaternion.identity, transform);
        go.GetComponent<SpriteRenderer>().sprite = cloudSprites[randomIndex];
        clouds.Add(go);
    }

    public void DeleteCloud()
    {
        List<GameObject> removeCloue = new();
        foreach(var cloud in clouds)
        {
            if(cloud.transform.position.x < transform.position.x - 0.64f)
            {
                removeCloue.Add(cloud);
            }
        }

        foreach(var cloud in removeCloue)
        {
            clouds.Remove(cloud);
            GameManager.Destroy(cloud);
        }
    }
}
