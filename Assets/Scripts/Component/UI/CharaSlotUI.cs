using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CharaSlotUI : Obj
{
    public int index;
    public Image charaIcon;
    public Image HPBar;

    public int currentCharaId;


    public float movingT = 0;
    public float enableXePos = 180, disableXPos = -310;

    RectTransform rect;

    public override void OnCreate()
    {
        base.OnCreate();
        rect = GetComponent<RectTransform>();
    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        try
        {
            
            Charactor targetCharactor = GameManager.Progress.charaDatas[GameManager.Progress.currentParty[index].charaData.id].charactor;
            if(targetCharactor != null)
            {
                gameObject.SetActive(true);
                HPBar.fillAmount = (float)targetCharactor.charaData.currentHP / (float)targetCharactor.charaData.maxHP;
                currentCharaId = GameManager.Progress.charaDatas[GameManager.Progress.currentParty[index].charaData.id].charactor.charaData.id;

            }
            else
            {
                
                gameObject.SetActive(false);
            }

        }
        catch
        {

            gameObject.SetActive(false);
        }
    }

    public async void Enable(float t = 0)
    {
        Debug.Log("Enable Charactor Slot UI");
        gameObject.SetActive(true); 
        movingT = t;
        while (movingT < 1f)
        {
            if(GameManager.GetUIState() != UIState.InPlay)
            {
                return;
            }
            float targetX = Mathf.Lerp(rect.localPosition.x, enableXePos - Screen.width / 2, movingT);
            rect.localPosition = new Vector3(targetX, rect.localPosition.y);
            movingT += Time.deltaTime * 2;
            await Task.Yield();
        }

        rect.localPosition = new Vector3(enableXePos - Screen.width / 2, rect.localPosition.y);
        movingT = 0f;
    }

    public async void Disable(float t = 0)
    {
        movingT = t;
        while (movingT < 1f)
        {
            if(GameManager.uiState == UIState.InPlay)
            {
                return;
            }
            float targetX = Mathf.Lerp(rect.localPosition.x, disableXPos - Screen.width / 2, movingT);
            rect.localPosition = new Vector3(targetX, rect.localPosition.y);
            movingT += Time.deltaTime * 2;
            await Task.Yield();
        }

        rect.localPosition = new Vector3(disableXPos - Screen.width / 2, rect.localPosition.y);
        movingT = 0f;
        gameObject.SetActive(false);
    }

    public void DisableAfterAnimation()
    {
        gameObject.SetActive(false);
    }

}
