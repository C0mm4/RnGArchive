using System;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



// 리소스의 Load, Instantiate, Destroy 를 관리하는 리소스 매니저. 
public class ResourceManager
{
    private Dictionary<string, AsyncOperationHandle> LoadResources = new();

    // path에 있느 파일을 로드하는 함수, 로드되는 조건은 Object 일 때
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    // Load Game Object by GameObject instance
    public GameObject Instantiate(GameObject gameObject)
    {
        GameObject prefab = UnityEngine.Object.Instantiate(gameObject);
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
        GameObject prefab = UnityEngine.Object.Instantiate(gameObject, pos, rotation);
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
                GameObject go = Instantiate((GameObject)LoadResources[path].Result, pos, rotation);
                return go;
            }
            else
            {
                var op = LoadAssetAsync<GameObject>(path);
                if(!op.Equals(default))
                {
                    LoadResources[path] = op;
                }
                if (LoadResources.ContainsKey(path))
                {
                    GameObject go = Instantiate((GameObject)LoadResources[path].Result, pos, rotation);
                    return go;

                }
                else
                {
                    Debug.Log($"Failed to load GameObject : {path}");
                    return null;
                }
            }
        }
    

    public AsyncOperationHandle LoadAssetAsync<T>(string path) where T : UnityEngine.Object
    {
        if (LoadResources.ContainsKey(path))
        {
            Debug.Log($"Already Loaded Resource : {path}");
            return LoadResources[path];
        }
        else
        {
            try
            {
                Debug.Log($"Load Resource : {path}");
                var op = Addressables.LoadAssetAsync<T>(path);
                op.WaitForCompletion();
                LoadResources[path] = op;
                return op;
            }
            catch
            {
                Debug.LogError($"Failed to load Resource : {path}");
                return default;
            }
        }
    }

    public AsyncOperationHandle LoadAssetAsync<T>(AssetReference assetReference) where T : UnityEngine.Object
    {
        var op = Addressables.LoadAssetAsync<T>(assetReference);
        op.WaitForCompletion();
        return op;
    }


    public List<T> LoadAssetAsyncWithTag<T>(string label) where T : UnityEngine.Object
    {
        List<T> ret = new();
        var ops = Addressables.LoadAssetsAsync<T>(label, prefab => ret.Add(prefab));

        return ret;
    }


    public Sprite LoadSprite(string path)
    {
        if (LoadResources.ContainsKey(path))
        {
            Sprite spr = (Sprite)LoadResources[path].Result;
            return spr;
        }
        else
        {
            var op = LoadAssetAsync<Sprite>(path);
            if (!op.Equals(default))
            {
                LoadResources[path] = op;
            }
            if (LoadResources.ContainsKey(path))
            {
                Sprite spr = (Sprite)LoadResources[path].Result;
                return spr;

            }
            else
            {
                Debug.Log($"Failed to load Sprite : {path}");
                return null;
            }
        }
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
            UnityEngine.Object.Destroy(obj);
        }
        else
        {
            Debug.Log("Destroy by Release");
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