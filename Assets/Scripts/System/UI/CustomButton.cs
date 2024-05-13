using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Obj, IPointerClickHandler
{

    public Button button;

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
//        GameManager.LoadGame();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
}