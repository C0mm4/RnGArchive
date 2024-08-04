using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContentsSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image charaImg;
    public Contents contents;

    public GameObject InfoUI;

    public void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CreateInfoUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInfoUI();
    }

    public virtual void SetContent(Contents content)
    {
        contents = content;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public virtual void OnFresh()
    {

    }

    public virtual void OnClick()
    {

    }

    public virtual void CreateInfoUI()
    {

    }

    public virtual void DestroyInfoUI() 
    {
        GameManager.Destroy(InfoUI);
    }
}
