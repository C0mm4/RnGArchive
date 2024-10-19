using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomTilemap : MonoBehaviour
{
    public static Tilemap tilemap;
    public static List<GameObject> prefabs;

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
        return null;
    }

    public List<Object> GetAllTiles()
    {
        List<Object> ret = new();

        BoundsInt bounds = tilemap.cellBounds;
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
        }

        ret.AddRange(prefabs);

        return ret;
    }
}
