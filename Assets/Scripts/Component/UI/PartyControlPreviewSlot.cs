using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartyControlPreviewSlot : ContentsSlot
{
    [SerializeField]
    Charactor charactor;

    public Image image;
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
        
        if (charactor != null)
        {
            image.color = new Color(1, 1, 1, 1);

            image.sprite = GameManager.Resource.LoadSprite(charactor.charaData.ProfileImg);
            text.text = charactor.charaData.Name;

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
        if(charactor != null)
        {
            originUI.selectParty.Remove(charactor);
        }

        OnFresh();
        originUI.SetPreview();
    }
}
