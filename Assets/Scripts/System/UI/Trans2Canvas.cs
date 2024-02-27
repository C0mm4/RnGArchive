using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trans2Canvas : MonoBehaviour
{
    public string UIPrefab;
    public GameObject UIIngameTransform;

    public GameObject UIObj;


    public void GenerateUI(string prefabPath)
    {
        var awaitObj = GameManager.InstantiateAsync(UIPrefab, GameManager.UIManager.canvas.transform.position);
        UIObj = awaitObj;
        Func.SetRectTransform(UIObj, GameManager.CameraManager.maincamera.WorldToScreenPoint(UIIngameTransform.transform.position) - new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f));
    }

    public void Update()
    {
        if (UIObj != null)
        {
            Func.SetRectTransform(UIObj, GameManager.CameraManager.maincamera.WorldToScreenPoint(UIIngameTransform.transform.position) - new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f));
        }
    }
}