using System.Xml;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



// 리소스의 Load, Instantiate, Destroy 를 관리하는 리소스 매니저. 
public class ResourceManager
{

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

    // Load Game Object by Prefab Path
    public GameObject Instantiate(string path)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }
        else
        {
            return Object.Instantiate(prefab);

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
        Object.Destroy(obj);
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