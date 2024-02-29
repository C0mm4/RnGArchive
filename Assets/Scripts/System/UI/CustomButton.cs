using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Obj
{

    Button button;

    public override void OnCreate()
    {
        try
        {
            button = GetComponent<Button>();

        }
        catch 
        {
            button = gameObject.AddComponent<Button>();
        }
        button.onClick.AddListener(OnClick);
    }

    public virtual void OnClick()
    {
        GameManager.GameStart();
    }
}
