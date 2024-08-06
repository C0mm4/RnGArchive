using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PartyControlUI : Menu
{
    public List<Charactor> openCharas = new();
    public List<Supporter> openSupporter = new();

    public List<RectTransform> viewPort;
    public List<RectTransform> contents;

    public ScrollRect scrollView;

    public ListContents strikerSlot;

    public ListContents specialSlot;

    public List<Charactor> selectParty;

    public Supporter selectSuport = new();

    public List<PartyControlSlotStriker> strikerSlots;

    public List<PartyControlPreviewSlot> selectedStriker;

    public List<PartyControlSlotSpecial> specialSlots;

    public PartyControlPreviewSlotSpe selectedSpecial;

    public int viewIndex;

    public override void OnCreate()
    {
        base.OnCreate();
        viewPort[1].gameObject.SetActive(false);
        var openList = GameManager.Progress.charaDatas.Values.ToList();
        foreach(var charactor in openList)
        {
            if(charactor.charactor.charaData.id != 10001000)
            {
                openCharas.Add(charactor.charactor);
                Debug.Log(charactor.charactor);
                strikerSlot.AddContents(charactor.charactor);

            }
        }
        strikerSlots = GetComponentsInChildren<PartyControlSlotStriker>(true).ToList();
        foreach (var obj in strikerSlots)
        {
            obj.originUI = this;
        }

        var openSpecial = GameManager.Progress.openSupporeters.ToList();

        foreach(var special in openSpecial)
        {
            Debug.Log(special.data.id);
            openSupporter.Add(special);
            specialSlot.AddContents(special);
        }

        specialSlots = GetComponentsInChildren<PartyControlSlotSpecial>(true).ToList();
        Debug.Log(specialSlots.Count);  
        foreach (var obj in specialSlots)
        {
            obj.originUI = this;
        }

        // Set Selected Party Data

        selectParty = GameManager.Progress.currentParty.ToList();
        Debug.Log(GameManager.Progress.currentSupporterId);
        selectSuport = GameManager.CharaCon.supporters[GameManager.Progress.currentSupporterId];
        Debug.Log(selectSuport.data.id);
        ViewStriker();
        SetPreview();
    }

    public override void ConfirmAction()
    {
        if(selectParty.Count >= 1)
        {
            if(selectSuport != null)
            {
                GameManager.Progress.currentParty = selectParty;
                GameManager.Progress.currentSupporterId = int.Parse(selectSuport.data.id);

                GameManager.UIManager.endMenu();
            }
            else
            {

            }
        }
        else
        {

        }
    }

    public void ViewStriker()
    {
        viewIndex = 0;
        scrollView.viewport = viewPort[0];
        scrollView.content = contents[0];

        strikerSlot.FreshUI();

        viewPort[1].gameObject.SetActive(false);
        viewPort[0].gameObject.SetActive(true);
    }

    public void ViewSpecial()
    {
        viewIndex = 1;
        scrollView.viewport = viewPort[1];
        scrollView.content = contents[1];

        specialSlot.FreshUI();

        viewPort[0].gameObject.SetActive(false);
        viewPort[1].gameObject.SetActive(true);
    }

    public void SetPreview()
    {
        int i = 0;

        foreach(var obj in strikerSlots)
        {
            obj.OnFresh();
        }

        for(; i < selectParty.Count; i++)
        {
            selectedStriker[i].SetContent(selectParty[i]);
            selectedStriker[i].OnFresh();
        }
        for(; i < 3; i++)
        {
            selectedStriker[i].SetContent(null);
            selectedStriker[i].OnFresh();
        }

        foreach(var obj in specialSlots)
        {
            obj.OnFresh();

        }
        selectedSpecial.SetContent(selectSuport);
        selectedSpecial.OnFresh();

    }
}
