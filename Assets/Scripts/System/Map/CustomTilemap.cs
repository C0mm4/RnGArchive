using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public List<GameObject> prefabs;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        
        prefabs = new();

        foreach(Transform child in transform)
        {
            prefabs.Add(child.gameObject);
        }
    }

    public Object GetObjectAtCell(Vector3Int cellPos)
    {
        TileBase tile = tilemap.GetTile(cellPos);
        if (tile != null)
        {
            return tile;
        }

        // RuleTile이 있는 경우를 확인하기 위해
        
        TileBase[] tiles = tilemap.GetTilesBlock(new BoundsInt(cellPos, Vector3Int.one));
        foreach (TileBase t in tiles)
        {

            if (t is RuleTile)
            {
                Debug.Log("RuleTile");
                return t;
            }
        }
        foreach (GameObject obj in prefabs)
        {
            if (tilemap.WorldToCell(obj.transform.position) == cellPos)
            {
                return obj;
            }
        }
        return null;
/*
        TileBase tile = tilemap.GetTile(cellPos);
        if(tile != null)
        {
            return tile;
        }
        else
        {
            foreach(GameObject obj in prefabs)
            {
                if(tilemap.WorldToCell(obj.transform.position) == cellPos)
                {
                    return obj;
                }
            }
        }
        return null;*/
    }

    public List<Object> GetAllTiles()
    {
        List<Object> ret = new();

        BoundsInt bounds = tilemap.cellBounds;
        
        var tiles = tilemap.GetTilesBlock(bounds);

        foreach (Object t in tiles)
        {
            ret.Add(t);
        }
        /*
        for(int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for(int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                Object obj = GetObjectAtCell(pos);
                if(obj != null)
                {
                    ret.Add(obj);
                }
            }
        }*/

        ret.AddRange(prefabs);

        return ret;
    }
}
