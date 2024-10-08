using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

[Serializable]
public class CharactorController
{
    public Dictionary<string, Charactor> _charactors;
    public Dictionary<string, Charactor> charactors { get { return _charactors; } }

    public Dictionary<string, Supporter> _supporters;
    public Dictionary<string, Supporter> supporters { get { return _supporters; } }

    public List<Charactor> charas;

    public string charactorDataXML = "charaData";
    public string supportDataXML = "Supporter";

    XmlDocument text;

    public List<Supporter> supporterss;

    public void initialize()
    {
        text = GameManager.Resource.LoadXML(charactorDataXML);
        _charactors = new();
        foreach(XmlNode node in text.SelectNodes("/Root/text/Charactor"))
        {

            Type T = Type.GetType(node["class"].InnerText);
            Charactor newChara = Activator.CreateInstance(T) as Charactor;
            CharactorData charaData = new();

            charaData.id = node["id"].InnerText;

            charaData.attackSpeed = float.Parse(node["AttackSpeed"].InnerText);

            charaData.maxSpeed = float.Parse(node["maxSpeed"].InnerText);
            charaData.jumpForce = float.Parse(node["jumpForce"].InnerText);

            charaData.maxHP = int.Parse(node["baseHP"].InnerText);
            charaData.currentHP = charaData.maxHP;


            charaData.maxAmmo = int.Parse(node["MaxAmmo"].InnerText);
            charaData.currentAmmo = charaData.maxAmmo;

            charaData.reloadT = float.Parse(node["reloadT"].InnerText);

            charaData.prefabPath = node["path"].InnerText;

            
            
            charaData.skins = new();

            XmlNode name = node.SelectSingleNode("name").SelectSingleNode(GameManager.gameData.Language[GameManager.gameData.LanguageIndex]);

            charaData.Name = name.InnerText;

            foreach (XmlNode skin in node.SelectNodes("Skins/Skin"))
            {
                charaData.skins.Add(skin.InnerText);
            }

            charaData.haloSkins = new();

            foreach(XmlNode skin in node.SelectNodes("Halos/Halo"))
            {
                charaData.haloSkins.Add(skin.InnerText);

            }

            charaData.AtkType = node["AtkType"].InnerText;
            charaData.PassiveName = node["PassiveName"][GameManager.gameData.Language[GameManager.gameData.LanguageIndex]].InnerText;
            charaData.PassiveImg = node["PassiveImg"].InnerText;
            charaData.PassiveTooltip = node["PassiveTooltip"][GameManager.gameData.Language[GameManager.gameData.LanguageIndex]].InnerText;


            XmlNode Skills = node.SelectSingleNode("Skills");
            foreach(XmlNode skill in Skills.SelectNodes("Skill"))
            {
                Type skl = Type.GetType(skill["class"].InnerText);
                Skill target = Activator.CreateInstance(skl) as Skill;
                target.name = skill[GameManager.gameData.Language[GameManager.gameData.LanguageIndex]].InnerText;
                target.type = skill["Type"].InnerText;
                target.info = skill["SkillTooltip"][GameManager.gameData.Language[GameManager.gameData.LanguageIndex]].InnerText;
                target.imgPath = skill["SkillImg"].InnerText;
                target.coolTime = float.Parse(skill["CoolTime"].InnerText);

                newChara.skill = target;

            }

            newChara.SetType(node["AtkType"].InnerText, node["DefType"].InnerText);

            charaData.ProfileImg = node.SelectSingleNode("ProfileImg").InnerText;
            Debug.Log(node.SelectSingleNode("ProfileImg").InnerText);
            

            newChara.charaData = charaData;

            charactors[node["id"].InnerText] = newChara;

        }

        

        _supporters = new();
        text = GameManager.Resource.LoadXML(supportDataXML);
        foreach(XmlNode node in text.SelectNodes("/Root/text/Charactor"))
        {
            Supporter supporter = new Supporter();
            SupporterData data = new SupporterData();
            data.id = node["id"].InnerText;
            data.name = node["name"][GameManager.gameData.Language[GameManager.gameData.LanguageIndex]].InnerText;

            data.maxHP = int.Parse(node["maxHP"].InnerText);
            data.coolTime = float.Parse(node["coolTime"].InnerText);
            data.objPath = node["objPath"].InnerText;
            data.ProfileImg = node["ProfileImg"].InnerText;
            data.SkillName = node["SkillName"][GameManager.gameData.Language[GameManager.gameData.LanguageIndex]].InnerText;
            data.atkType = node["AtkType"].InnerText;
            data.defType = node["DefType"].InnerText;
            data.SkillImg = node["SkillImg"].InnerText;
            data.durateT = float.Parse(node["durateT"].InnerText);
            data.info = node["SkillTooltip"][GameManager.gameData.Language[GameManager.gameData.LanguageIndex]].InnerText;
            supporter.data = data;

            supporters[data.id] = supporter;
        }

        charas = charactors.Values.ToList();

        supporterss = supporters.Values.ToList();
    }


    public void Update()
    {
        if(GameManager.Progress != null)
        {
            if (supporters.ContainsKey(GameManager.Progress.currentSupporterId))
            {
                Supporter supporter = supporters[GameManager.Progress.currentSupporterId];

                if (supporter.isCool)
                {
                    supporter.data.leftCoolTime -= Time.deltaTime;
                    if (supporter.data.leftCoolTime < 0)
                    {
                        supporter.isCool = false;
                    }
                }

            }

        }
    }
}
