using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class InGameUI : Obj
{
    public List<GameObject> charas;
    public List<GameObject> skillSlots;

    public bool isShow;

    public override void OnCreate()
    {
        base.OnCreate();
        Func.SetRectTransform(gameObject);
        isShow = false;
    }

    public void EnableUI()
    {
        EnableCharaSlots();
        isShow = true;
    }

    public void DisableUI()
    {
        DisableCharaSlots();
        isShow = false;
    }

    public void EnableCharaSlots()
    {
        if (!isShow)
        {
            int i;
            for (i = 0; i < GameManager.Progress.currentParty.Count; i++)
            {
                charas[i].GetComponent<CharaSlotUI>().Enable();
            }
            for (; i < charas.Count; i++)
            {
                charas[i].gameObject.SetActive(false);
            }
        }
    }

    public void DisableCharaSlots()
    {
        if(isShow)
        {
            int i;
            for (i = 0; i < GameManager.Progress.currentParty.Count; i++)
            {
                charas[i].GetComponent<CharaSlotUI>().Disable();
            }

        }
    }

    public async void CharactorChange(int target1, int target2)
    {
        charas[target1].GetComponent<CharaSlotUI>().Disable(0, 0.2f);
        charas[target2].GetComponent<CharaSlotUI>().Disable(0, 0.2f);

        await Task.Delay(TimeSpan.FromMilliseconds(300));
        Func.Swap(GameManager.Progress.currentParty, target1, target2);
        
        
        charas[target1].GetComponent<CharaSlotUI>().Enable(0, 0.2f);
        charas[target2].GetComponent<CharaSlotUI>().Enable(0, 0.2f);
    }

}
