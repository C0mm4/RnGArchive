using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContentsSlot : CustomButton
{
    public Image charaImg;
    public Contents contents;

    public GameObject InfoUI;


    public override void OnCreate()
    {
        base.OnCreate();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        DestroyInfoUI();
    }

    public virtual void SetContent(Contents content)
    {
        contents = content;
    }


    public virtual void OnFresh()
    {

    }


    public virtual void CreateInfoUI()
    {
        InfoUI = GameManager.InstantiateAsync("InfoUI");
        Func.SetRectTransform(InfoUI);
    }

    public virtual void DestroyInfoUI() 
    {
        if(InfoUI != null) 
        {
            GameManager.Destroy(InfoUI);
        }
        
    }


}
