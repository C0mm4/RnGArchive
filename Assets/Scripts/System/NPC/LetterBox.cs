using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterBox : Obj
{
    public string text;
    public TMP_Text txt;
    public Image TopLetterBox;
    public Image MiddleLetterBox;
    public Image LowerLetterBox;

    public NPC npc;

    public void SetText(string script)
    {
        text = script;
        txt.text = text;
    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        if(npc == null)
        {
            Destroy();
        }
        else
        {
            SetPosition();
        }
    }

    public override void AfterStep()
    {
        base.AfterStep();
        MiddleLetterBox.GetComponent<RectTransform>().sizeDelta = new Vector2(MiddleLetterBox.GetComponent<RectTransform>().sizeDelta.x, txt.preferredHeight);
        TopLetterBox.GetComponent<RectTransform>().localPosition = new Vector2(TopLetterBox.GetComponent<RectTransform>().localPosition.x, MiddleLetterBox.GetComponent<RectTransform>().sizeDelta.y + 100f);
    }

    public void SetPosition()
    {
        var screenPosition = GameManager.CameraManager.maincamera.WorldToScreenPoint(npc.transform.position);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameManager.UIManager.canvas.GetComponent<RectTransform>(), screenPosition, null, out localPoint);

        var rect = GetComponent<RectTransform>();
        rect.anchoredPosition = localPoint + new Vector2(0, 100);
    }
}
