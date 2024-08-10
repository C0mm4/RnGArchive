using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyControlSlotSpecial : ContentsSlot
{

    [SerializeField]
    Supporter charactor;

    public Image image;

    public Image isSelect;

    public bool isSelected;

    public TMP_Text text;

    public PartyControlUI originUI;

    public override void SetContent(Contents content)
    {
        base.SetContent(content);
        charactor = content as Supporter;
    }

    public override void OnFresh()
    {
        base.OnFresh();

        if(charactor != null)
        {
            image.color = new Color(1, 1, 1, 1);
            if(originUI.selectSuport != null)
            {
                if (originUI.selectSuport.data.id.Equals(charactor.data.id))
                {

                    isSelected = true;
                }
                else
                {
                    isSelected = false;
                }

            }
            else
            {
                isSelected = false;
            }

            image.sprite = GameManager.Resource.LoadSprite(charactor.data.ProfileImg);
            text.text = charactor.data.name;

            if (isSelected)
            {
                isSelect.color = new Color(0f, 0f, 0f, 0.8f);

            }
            else
            {
                isSelect.color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            image.color = new Color(0, 0, 0, 0);
            text.text = string.Empty;
        }
    }

    public override void OnClick()
    {
        if(originUI.viewIndex == 0)
        {
            originUI.ViewSpecial();
        }
        if (isSelected)
        {
            originUI.selectSuport = null;
            isSelected = !isSelected;
        }
        else
        {
            if (originUI.selectSuport == null) 
            { 
                originUI.selectSuport = charactor;
                isSelected = !isSelected;
            }
        }
        OnFresh();

        originUI.SetPreview();
    }
    public override void CreateInfoUI()
    {
        base.CreateInfoUI();
        InfoUI.GetComponent<InfoUI>().SetData(charactor);
    }
}
