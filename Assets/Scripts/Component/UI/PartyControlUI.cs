using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PartyControlUI : Menu
{
    public List<Charactor> openCharas;
    public List<GameObject> openCharaSlots;

    public List<RectTransform> viewPort;
    public List<RectTransform> contents;

    public ScrollRect scrollView;

    public override void OnCreate()
    {
        base.OnCreate();
        viewPort[1].gameObject.SetActive(false);
        var openList = GameManager.Progress.charaDatas.Values.ToList();
        foreach(var charactor in openList)
        {
            if(charactor.charactor.charaData.id != 10001000)
                openCharas.Add(charactor.charactor);
        }
    }

    public override void ConfirmAction()
    {

    }

    public void ViewStriker()
    {
        scrollView.viewport = viewPort[0];
        scrollView.content = contents[0];
        viewPort[1].gameObject.SetActive(false);
        viewPort[0].gameObject.SetActive(true);
    }

    public void ViewSpecial()
    {
        scrollView.viewport = viewPort[1];
        scrollView.content = contents[1];
        viewPort[0].gameObject.SetActive(false);
        viewPort[1].gameObject.SetActive(true);
    }
}
