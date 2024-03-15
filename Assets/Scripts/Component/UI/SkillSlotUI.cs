using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : Obj
{
    public Skill targetSkill;
    public int fullAmmo;
    public int currentAmmo;

    public Image img;
    public TMP_Text ammoText;


    public void SetData()
    {
        fullAmmo = targetSkill.maxAmmo;
        currentAmmo = targetSkill.currentAmmo;

        ammoText.text =  currentAmmo + " / " + fullAmmo;
    }
}
