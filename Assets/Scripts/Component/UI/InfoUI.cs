using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    [Serializable]
    public class SkillInfo
    {
        public Image image;
        public TMP_Text name;
        public TMP_Text type;
        public TMP_Text info;
    }

    public SkillInfo Passive;
    public SkillInfo EXSkill;


    public RectTransform rt;
    public GameObject PassiveSkillInfoObj;
    public GameObject EXSkillInfoObj;

    public void SetData(Charactor charactor)
    {
        
        Passive.image.sprite = GameManager.Resource.LoadSprite(charactor.charaData.PassiveImg);
        Passive.name.text = charactor.charaData.PassiveName;
        Passive.type.text = charactor.charaData.AtkType;
        Passive.info.text = charactor.charaData.PassiveTooltip;

        EXSkill.image.sprite = GameManager.Resource.LoadSprite(charactor.skill.imgPath);
        EXSkill.name.text = charactor.skill.name;
        EXSkill.type.text = charactor.skill.type;
        EXSkill.info.text = charactor.skill.info;
    }

    public void SetData(Supporter charactor)
    {
        PassiveSkillInfoObj.SetActive(false);

        rt.sizeDelta += new Vector2(0, -350);
        EXSkillInfoObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        EXSkill.image.sprite = GameManager.Resource.LoadSprite(charactor.data.SkillImg);
        EXSkill.name.text = charactor.data.SkillName;
        EXSkill.type.text = charactor.data.atkType;
        EXSkill.info.text = charactor.data.info;


    }
}
