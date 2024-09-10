using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartyControlPreviewSlot : ContentsSlot
{
    [SerializeField]
    public Charactor charactor;

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

            image.sprite = GameManager.LoadSprite(charactor.charaData.ProfileImg);
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

        if (charactor != null)
        {
            if (originUI.viewIndex == 1)
            {
                originUI.ViewStriker();

            }
            if (GameManager.Progress.currentParty[0].charaData.id == charactor.charaData.id)
            {

                GameManager.UIManager.SetText("해당 캐릭터를 변경할 수 없습니다.");
            }
            else
            {
                originUI.selectParty.Remove(charactor);
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
