using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;


public class CharactorController
{
    public Dictionary<int, Charactor> _charactors;
    public Dictionary<int, Charactor> charactors { get { return _charactors; } }

    public Dictionary<int, Supporter> _supporters;
    public Dictionary<int, Supporter> supporters { get { return _supporters; } }

    public string charactorDataXML = "charaData";
    public string supportDataXML = "Supporter";

    XmlDocument text;

    public void initialize()
    {
        text = GameManager.Resource.LoadXML(charactorDataXML);
        _charactors = new();
        foreach(XmlNode node in text.SelectNodes("/Root/text/Charactor"))
        {

            Type T = Type.GetType(node["class"].InnerText);
            Charactor newChara = Activator.CreateInstance(T) as Charactor;
            CharactorData charaData = new();

            charaData.id = int.Parse(node["id"].InnerText);

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

            newChara.commands = new();

            XmlNode Skills = node.SelectSingleNode("Skills");
            foreach(XmlNode skill in Skills.SelectNodes("Skill"))
            {
                Type skl = Type.GetType(skill["class"].InnerText);
                Skill target = Activator.CreateInstance(skl) as Skill;
                target.name = skill[GameManager.gameData.Language[GameManager.gameData.LanguageIndex]].InnerText;

                target.coolTime = float.Parse(skill["CoolTime"].InnerText);

                newChara.skill = target;

            }

            newChara.SetType(node["AtkType"].InnerText, node["DefType"].InnerText);

            charaData.ProfileImg = node.SelectSingleNode("ProfileImg").InnerText;
            Debug.Log(node.SelectSingleNode("ProfileImg").InnerText);
            

            newChara.charaData = charaData;

            charactors[int.Parse(node["id"].InnerText)] = newChara;

            Debug.Log(newChara.charaData.ProfileImg);
        }

        

        _supporters = new();
        text = GameManager.Resource.LoadXML(supportDataXML);
        foreach(XmlNode node in text.SelectNodes("/Root/text/Charactor"))
        {
            Supporter supporter = new Supporter();
            SupporterData data = new SupporterData();
            data.id = node["id"].InnerText;
            data.name = node["name"].InnerText;
            data.maxAmmo = int.Parse(node["maxAmmo"].InnerText);
            data.currentAmmo = data.maxAmmo;
            data.maxHP = int.Parse(node["maxHP"].InnerText);
            data.objName = node["objName"].InnerText;
            data.coolTime = float.Parse(node["coolTime"].InnerText);
            data.cost = int.Parse(node["cost"].InnerText);
            data.ProfileImg = node["ProfileImg"].InnerText;
            supporter.data = data;

            supporters[int.Parse(data.id)] = supporter;
        }
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
                        Debug.Log("Cool is Done");
                    }
                }

            }

        }
    }
}
