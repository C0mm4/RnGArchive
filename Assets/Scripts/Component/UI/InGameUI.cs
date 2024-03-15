using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameUI : Obj
{
    public List<GameObject> charas;
    public List<GameObject> skillSlots;


    public override void OnCreate()
    {
        base.OnCreate();
        Func.SetRectTransform(gameObject);
        DisableSkillSLots();
    }

    public void EnableUI()
    {
        EnableCharaSlots();
        EnableSkillSlots();
    }

    public void DisableUI()
    {
        DisableCharaSlots();
        DisableSkillSLots();
    }

    public void EnableCharaSlots()
    {
        int i;
        for(i = 0; i < GameManager.Progress.currentParty.Count; i++) 
        {
            charas[i].gameObject.SetActive(true);
        }
        for(; i < charas.Count; i++)
        {
            charas[i].gameObject.SetActive(false);
        }
    }

    public void DisableCharaSlots()
    {
        foreach(var chara in charas)
        {
            chara.gameObject.SetActive(false);
        }
    }

    public void EnableSkillSlots()
    {
        for(int i = 0; i < skillSlots.Count; i++)
        {
            if (skillSlots[i] != null)
            {
                skillSlots[i].GetComponent<SkillSlotUI>().targetSkill = GameManager.player.GetComponent<PlayerController>().charactor.skills[i];
                skillSlots[i].GetComponent<SkillSlotUI>().SetData();
                skillSlots[i].gameObject.SetActive(true);
            }
        }
    }

    public void DisableSkillSLots()
    {
        foreach(var slot in skillSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }
}
