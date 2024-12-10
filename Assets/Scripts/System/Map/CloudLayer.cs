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
    public List<Sprite> specialClouds;

    public override void OnCreate()
    {
        base.OnCreate();

        tilemapRightMax = transform.position.x + 22 * 0.32f; 
        maxY = GetComponent<Tilemap>().cellBounds.size.y * 0.32f;
        minY = maxY / 2;
        nextT = 0;
        if (!GameManager.isPaused)
        {
            CloudInitialize();
        }
    }

    public override void Step()
    {
        base.Step();

        tilemapRightMax = transform.position.x + 22 * 0.32f;
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
            cloud.transform.position -= new Vector3(cloudSpd, 0, 0) * Time.deltaTime * Random.Range(0.7f, 1.25f);
        }
    }
    public void CloudInitialize()
    {
        float xMin, xMax;
        xMin = 0;
        xMax = tilemapRightMax + 0.64f;

        for (int i = 0; i < GetComponent<Tilemap>().size.x; i += 2)
        {
            int attempt = 0;
            bool isPositionValid;

            int randomIndex = UnityEngine.Random.Range(0, 10);
            if(randomIndex >= 9)
            {
                randomIndex = UnityEngine.Random.Range(0, specialClouds.Count);
            }
            else
            {
                randomIndex = UnityEngine.Random.Range(0, cloudSprites.Count);
            }

            isPositionValid = true;

            float randomX, randomY;
            Vector3 pos;

            do
            {
                randomX = UnityEngine.Random.Range(xMin, xMax);
                randomY = UnityEngine.Random.Range(minY, maxY);


                pos = new Vector3(randomX, randomY, 9);

                // if clouds so close, refind new position
                foreach (var cloud in clouds)
                {
                    if (Vector2.Distance(cloud.transform.position, pos) <= 0.48f)
                    {
                        isPositionValid = false;
                        break;
                    }
                }
                attempt++;

            } while (!isPositionValid && attempt < 10);

            // if find valid new position, create new cloud
            if (isPositionValid)
            {
                GameObject go = Instantiate(cloudPrefab, pos, Quaternion.identity, transform);
                go.GetComponent<SpriteRenderer>().sprite = cloudSprites[randomIndex];
                clouds.Add(go);
            }
        }

    }


    public void CreateCloud()
    {
        float newCloudX = tilemapRightMax + 0.64f;
        int randomIndex = UnityEngine.Random.Range(0, 10);
        if (randomIndex >= 9)
        {
            randomIndex = UnityEngine.Random.Range(0, specialClouds.Count);
        }
        else
        {
            randomIndex = UnityEngine.Random.Range(0, cloudSprites.Count);
        }
        bool isPositionValid;
        int attempt = 0;

        float randomY;
        Vector3 pos;

        do
        {
            randomY = Random.Range(minY, maxY);
            pos = new Vector3(newCloudX, randomY, 9);

            isPositionValid = true;
            foreach (var cloud in clouds)
            {
                if (Vector2.Distance(cloud.transform.position, pos) <= 0.48f)
                {
                    isPositionValid = false;
                    break;
                }
            }
            attempt++;

        } while (attempt < 10 && isPositionValid);

        if (isPositionValid)
        {

            GameObject go = Instantiate(cloudPrefab, pos, Quaternion.identity, transform);
            go.GetComponent<SpriteRenderer>().sprite = cloudSprites[randomIndex];
            clouds.Add(go);
        }
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
