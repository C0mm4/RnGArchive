using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyControlSlotStriker : ContentsSlot
{

    [SerializeField]
    Charactor charactor;

    public Image image;

    public Image isSelect;

    public bool isSelected;

    public TMP_Text text;

    public PartyControlUI originUI;

    public override void SetContent(Contents content)
    {
        base.SetContent(content);
        charactor = content as Charactor;
    }

    public override void OnFresh()
    {
        base.OnFresh();

        if(charactor != null)
        {
            image.color = new Color(1, 1, 1, 1);
            if (originUI.selectParty.Exists(item => item.charaData.id == charactor.charaData.id))
            {
                isSelected = true;
            }
            else
            {
                isSelected = false;
            }

            image.sprite = GameManager.Resource.LoadSprite(charactor.charaData.ProfileImg);
            text.text = charactor.charaData.Name;

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
        if (isSelected)
        {
            originUI.selectParty.Remove(charactor);
        }
        else
        {
            originUI.selectParty.Add(charactor);
        }
        isSelected = !isSelected;
        OnFresh();

        originUI.SetPreview();
    }

}
