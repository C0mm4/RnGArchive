using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



// 리소스의 Load, Instantiate, Destroy 를 관리하는 리소스 매니저. 
public class ResourceManager
{
    public Dictionary<string, Object> LoadResources = new();

    // path에 있느 파일을 로드하는 함수, 로드되는 조건은 Object 일 때
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    // Load Game Object by GameObject instance
    public GameObject Instantiate(GameObject gameObject)
    {
        GameObject prefab = Object.Instantiate(gameObject);
        if(prefab != null)
        {
            return prefab;
        }
        else
        {
            Debug.Log($"Failed to load Prefab : {gameObject.name}");
            return null;
        }
    }

    public GameObject Instantiate(GameObject gameObject, Vector3 pos, Quaternion rotation)
    {
        GameObject prefab = Object.Instantiate(gameObject, pos, rotation);
        if (prefab != null)
        {
            return prefab;
        }
        else
        {
            Debug.Log($"Failed to load Prefab : {gameObject.name}");
            return null;
        }
    }
    
    public GameObject InstantiateAsync(string path, Vector3 pos = default, Quaternion rotation = default)
    {
        if (LoadResources.ContainsKey(path))
        {
            GameObject go = Instantiate((GameObject)LoadResources[path], pos, rotation);
            return go;
        }
        else
        {
            var op = Addressables.LoadAssetAsync<GameObject>(path);
            op.WaitForCompletion();
            LoadResources[path] = op.Result;
            Debug.Log(op.Result);
            GameObject go = Instantiate(op.Result, pos, rotation);
            return go;
        }
    }


    // Loading Sprites in path
    public Sprite LoadSprite(string path)
    {
        Sprite spr;
        spr = Load<Sprite>($"Sprites/{path}");
        if (spr == null)
        {
            Debug.Log($"Failed to load Sprite : {path}");
            spr = Load<Sprite>($"Sprites/default");
            if (spr == null)
            {
                Debug.Log($"Failed to load Sprite : default");
            }
            Debug.Log(spr.name);
            return spr;
        }

        return spr;
    }

    // Loading XML Datas in path
    public XmlDocument LoadXML(string path)
    {
        XmlDocument xml = new XmlDocument();
        TextAsset txtAsset = Load<TextAsset>($"XML/{path}");
        xml.LoadXml(txtAsset.text);

        if (xml == null)
        {
            Debug.Log($"Failed to load XML : {path}");
            return null;
        }

        return xml;
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null) return;
        if (!Addressables.ReleaseInstance(obj))
        {
            Object.Destroy(obj);
        }
    }

    public void Destroy(GameObject[] objs)
    {
        foreach(GameObject go in objs)
        {
            if(go != null)
                Destroy(go);
        }
    }

}