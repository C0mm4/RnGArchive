using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartyControlPreviewSlotSpe : ContentsSlot
{
    [SerializeField]
    public Supporter charactor;

    public Image image;
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
        
        if (charactor != null)
        {
            image.color = new Color(1, 1, 1, 1);

            image.sprite = GameManager.Resource.LoadSprite(charactor.data.ProfileImg);
            text.text = charactor.data.name;

        }
        else
        {
            image.color = new Color(0, 0, 0, 0);
            text.text = string.Empty;
        }
    }

    public override void OnClick()
    {
        base.OnClick();

        if (charactor != null)
        {
            if (originUI.viewIndex == 0)
            {
                originUI.ViewSpecial();
            }
            originUI.selectSuport = null;
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
