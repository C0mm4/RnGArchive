using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PartyControlUI : Menu
{
    public GameObject strikerButton;
    public GameObject specialButton;

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


    public async override void OnCreate()
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
            obj.menu = this;
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
            obj.menu = this;
        }

        // Set Selected Party Data

        selectParty = GameManager.Progress.currentParty.ToList();
        Debug.Log(GameManager.Progress.currentSupporterId);
        selectSuport = GameManager.CharaCon.supporters[GameManager.Progress.currentSupporterId];
        Debug.Log(selectSuport.data.id);
        ViewStriker();
        SetPreview();


        await Task.Delay(TimeSpan.FromSeconds(0.5f));

        OnMouseEnterHandler();
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
                GameManager.UIManager.SetText("Special을 선택해야 합니다.");
            }
        }
        else
        {
            GameManager.UIManager.SetText("파티는 1명 이상의 스트라이커와 스페셜을 요구합니다");
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

        cursorIndex = 0;
    }

    public void ViewSpecial()
    {
        viewIndex = 1;
        scrollView.viewport = viewPort[1];
        scrollView.content = contents[1];

        specialSlot.FreshUI();

        viewPort[0].gameObject.SetActive(false);
        viewPort[1].gameObject.SetActive(true);

        cursorIndex = 0;
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

    public override void KeyInputAlways()
    {
        base.KeyInputAlways();
        if(isGetInput && !isHoveringAnimation)
        {
            var currentIndex = cursorIndex;
            var lastButton = FindIndexButton(currentIndex);
            if (Input.GetKeyDown(GameManager.Input._keySettings.upKey))
            {
                // Top of Lists, Preview StrikerButton
                if ((cursorIndex >= 0 && cursorIndex < 4) || (cursorIndex >= 90 && cursorIndex < 93))
                {
                    cursorIndex = -2;
                }
                // StrikerButton, SpecialButton
                else if (cursorIndex == -2 || cursorIndex == -3)
                {
                    cursorIndex = -1;
                }
                // Close Button
                else if (cursorIndex == -1)
                {

                }
                // Preview Special Button
                else if (cursorIndex == 100)
                {
                    cursorIndex = 90;
                }
                // Confirm Button
                else if(cursorIndex == 999)
                {
                    cursorIndex = 100;
                }
                // List Contents not Top
                else
                {
                    cursorIndex -= 4;
                }
            }

            if (Input.GetKeyDown(GameManager.Input._keySettings.downKey))
            {
                // Preview StrikerButton
                if ((cursorIndex >= 90 && cursorIndex < 93))
                {
                    cursorIndex = 100;
                }
                // StrikerButton, SpecialButton
                else if (cursorIndex == -2 || cursorIndex == -3)
                {
                    cursorIndex = 0;
                }
                // Close Button
                else if (cursorIndex == -1)
                {
                    cursorIndex = -2;
                }
                // Preview Special Button
                else if (cursorIndex == 100)
                {
                    cursorIndex = 999;
                }
                // Confirm Button
                else if (cursorIndex == 999)
                {

                }
                // List Contents
                else
                {
                    cursorIndex += 4;
                    if(viewIndex == 0)
                    {
                        if(cursorIndex >= strikerSlots.Count)
                        {
                            cursorIndex = strikerSlots.Count -1;
                        }
                    }
                    else if(viewIndex == 1)
                    {
                        if(cursorIndex >= specialSlots.Count)
                        {
                            cursorIndex = specialSlots.Count -1;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(GameManager.Input._keySettings.leftKey))
            {
                // Close Button
                if (cursorIndex == -1)
                {
                    cursorIndex = -3;
                }
                // Special Button
                else if(cursorIndex == -3)
                {
                    cursorIndex = -2;
                }
                // Leftest Preview Striker, Special
                else if(cursorIndex == 90 || cursorIndex == 100)
                {
                    cursorIndex = -3;
                }
                // Confirm Button
                else if(cursorIndex == 999)
                {
                    cursorIndex = 100;
                }
                // Contents
                else if ((cursorIndex > 0 && cursorIndex < 89) || (cursorIndex == 91 || cursorIndex == 92))
                {
                    cursorIndex--;
                }
            }

            if(Input.GetKeyDown(GameManager.Input._keySettings.rightKey))
            {
                // Striker Button
                if(cursorIndex == -2)
                {
                    cursorIndex = -3;
                }
                // Preview Striker Slot Not Rightest
                else if(cursorIndex == 90 || cursorIndex == 91)
                {
                    cursorIndex++;
                }
                // Preview Special, Preview Striker rightest
                else if(cursorIndex == 100 || cursorIndex == 92)
                {
                    cursorIndex = 999;
                }
                // Special Button, rightest Contents
                else if(cursorIndex == -3 || (cursorIndex % 4 == 0 && cursorIndex != 0))
                {
                    cursorIndex = 90;
                }
                else if(cursorIndex >= 0 && cursorIndex < 89)
                {
                    if(viewIndex == 0)
                    {
                        if(cursorIndex == strikerSlots.Count - 1)
                        {
                            cursorIndex = 90;
                        }
                        else
                        {
                            cursorIndex++;
                        }
                    }
                    else if(viewIndex == 1)
                    {
                        if(cursorIndex == specialSlots.Count - 1)
                        {
                            cursorIndex = 90;
                        }
                        else
                        {
                            cursorIndex++;
                        }
                    }

                }


            }

            if(currentIndex != cursorIndex)
            {

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                lastButton.pointerExitEvent(pointerEventData);
                OnMouseEnterHandler();
            }

            if (Input.GetKeyDown(GameManager.Input._keySettings.Skill3))
            {
                if(currentIndex >= 0 && currentIndex < 90)
                {
                    if(viewIndex == 0)
                    {
                        strikerSlots[currentIndex].CreateInfoUI();
                    }
                    else if(viewIndex == 1)
                    {
                        specialSlots[currentIndex].CreateInfoUI();
                    }
                }
                else if(currentIndex >= 90 && currentIndex < 93)
                {
                    var index = currentIndex - 90;
                    if (selectedStriker[index].charactor != null)
                        selectedStriker[index].CreateInfoUI();
                }
                else if(currentIndex == 100)
                {
                    if(selectedSpecial.charactor != null)
                        selectedSpecial.CreateInfoUI();
                }
            }

            if (Input.GetKeyDown(GameManager.Input._keySettings.Interaction))
            {
                if(currentIndex == -1)
                {
                    closeButton.OnClick();
                }
                else if(currentIndex == -2)
                {
                    strikerButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                }
                else if(currentIndex == -3)
                {
                    specialButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                }
                else if(currentIndex >= 90 && currentIndex < 93)
                {
                    selectedStriker[currentIndex-90].GetComponent<CustomButton>().OnClick();

                }
                else if(currentIndex == 100)
                {
                    selectedSpecial.GetComponent<CustomButton>().OnClick();
                }
                else if(currentIndex == 999)
                {
                    confirmButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                }
                else
                {
                    if(viewIndex == 0)
                    {
                        strikerSlots[currentIndex].OnClick();
                    }
                    else if(viewIndex == 1)
                    {
                        specialSlots[currentIndex].OnClick();
                    }
                }
            }
        }

    }

    public override HoveringRectTransform FindIndexButton(int index)
    {
        CustomButton targetButton;
        if (cursorIndex == -1)
        {
            targetButton = closeButton.GetComponent<CustomButton>();
        }
        // cursor Index is Striker Slot Button
        else if (cursorIndex == -2)
        {
            targetButton = strikerButton.GetComponent<CustomButton>();
        }
        // cursor Index is Special Slot Button
        else if (cursorIndex == -3)
        {
            targetButton = specialButton.GetComponent<CustomButton>();

        }
        // cursor Index is Striker Preview
        else if (cursorIndex == 90 || cursorIndex == 91 || cursorIndex == 92)
        {
            var tmpindex = cursorIndex - 90;
            targetButton = selectedStriker[tmpindex].GetComponent<CustomButton>();
        }
        // cursor Index is Special Preview
        else if (cursorIndex == 100)
        {
            targetButton = selectedSpecial.GetComponent<CustomButton>();
        }
        else if (cursorIndex == 999)
        {
            targetButton = confirmButton.GetComponent<CustomButton>();
        }
        // cursor Index is Charactor Slots
        else
        {
            List<ContentsSlot> targetContents;

            if (viewIndex == 0)
            {
                targetContents = new List<ContentsSlot>(strikerSlots);
            }
            else
            {
                targetContents = new List<ContentsSlot>(specialSlots);
            }

            targetButton = targetContents[cursorIndex].GetComponent<CustomButton>();
        }

        return targetButton;
    }
}
