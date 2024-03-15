using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSlotUI : Obj
{
    public int index;
    public Image charaIcon;
    public Image HPBar;

    public override void BeforeStep()
    {
        base.BeforeStep();
        try
        {
            Charactor targetCharactor = GameManager.CharaCon.charactors[GameManager.Progress.currentParty[index].charaData.id];
            if(targetCharactor != null)
            {
                gameObject.SetActive(true);
                HPBar.fillAmount = (float)targetCharactor.charaData.currentHP / (float)targetCharactor.charaData.maxHP;

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
}
