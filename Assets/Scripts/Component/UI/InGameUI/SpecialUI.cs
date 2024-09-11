using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SpecialUI : Obj
{
    public Supporter special;

    public Image specialImg;
    public Image coolImg;

    public TMP_Text coolTimeText;

    public RectTransform rect;
    public float enableXePos = 160, disableXPos = -310;

    public int state;

    public float movingT = 0f;

    public override void Step()
    {
        base.Step();
        if (special.isCool)
        {
            coolImg.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            coolImg.fillAmount = (1 - special.data.leftCoolTime / special.data.coolTime);
            string coolTimetxt;
            if(special.data.leftCoolTime >= 1f)
            {
                coolTimetxt = ((int)(Mathf.Floor(special.data.leftCoolTime))).ToString() + "s";
            }
            else
            {
                coolTimetxt = "0." + Mathf.Round(special.data.leftCoolTime * 10).ToString() + "s";
            }
            coolTimeText.text = coolTimetxt;
        }
        else
        {
            coolImg.color = new Color(0, 0, 0, 0);
            coolTimeText.text = "";
        }
    }

    public void SetData(Supporter spe)
    {
        special = spe;
    }

    public async void Enable(float t = 0, float time = 1f)
    {
        state = 1;
        float targetY = -250 - ((GameManager.Progress.currentParty.Count - 1) * 110);
        gameObject.SetActive(true);
        special = GameManager.CharaCon.supporters[GameManager.Progress.currentSupporterId];
        specialImg.sprite = GameManager.LoadSprite(special.data.SkillImg);
        movingT = t;
        while (movingT < time)
        {
            if (GameManager.GetUIState() != UIState.InPlay)
            {
                return;
            }
            float targetX = Mathf.Lerp(rect.localPosition.x, enableXePos - Screen.width / 2, movingT);
            rect.localPosition = new Vector3(targetX, targetY + Screen.height / 2);
            movingT += Time.deltaTime * 2;
            await Task.Yield();
        }

        rect.localPosition = new Vector3(enableXePos - Screen.width / 2, targetY + Screen.height / 2);
        movingT = 0f;

        state = 2;
    }

    public async void Disable(float t = 0, float time = 1f)
    {
        state = 3;
        float targetY = -250 - ((GameManager.Progress.currentParty.Count - 1) * 110);
        bool playerChange = false;
        if (GameManager.uiState == UIState.InPlay)
        {
            playerChange = true;
        }
        movingT = t;
        while (movingT < time)
        {
            if (GameManager.uiState == UIState.InPlay && !playerChange)
            {
                return;
            }
            float targetX = Mathf.Lerp(rect.localPosition.x, disableXPos - Screen.width / 2, movingT);
            rect.localPosition = new Vector3(targetX, targetY + Screen.height / 2);
            movingT += Time.deltaTime * 2;
            await Task.Yield();
        }

        rect.localPosition = new Vector3(disableXPos - Screen.width / 2, targetY + Screen.height / 2);
        movingT = 0f;
        special = null;
        gameObject.SetActive(false);
        state = 0;
    }
}
