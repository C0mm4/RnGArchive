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

    public string charactorDataXML = "charaData";

    XmlDocument text;

    public void initialize()
    {
        text = GameManager.Resource.LoadXML(charactorDataXML);
        _charactors = new();
        foreach(XmlNode node in text.SelectNodes("/Root/text/Charactor"))
        {

            Type T = Type.GetType(node["class"].InnerText);
            Charactor newChara = Activator.CreateInstance(T) as Charactor;
            CharactorData charaData = new CharactorData();

            charaData.id = int.Parse(node["id"].InnerText);

            charaData.attackSpeed = float.Parse(node["AttackSpeed"].InnerText);

            charaData.moveAccelSpeed = float.Parse(node["moveAccelSpeed"].InnerText);
            charaData.breakAccel = float.Parse(node["breakAccel"].InnerText);
            charaData.maxSpeed = float.Parse(node["maxSpeed"].InnerText);
            charaData.prefabPath = node["path"].InnerText;

            
            
            charaData.skins = new();

            XmlNode name = node.SelectSingleNode("name").SelectSingleNode(GameManager.gameData.Language[GameManager.gameData.LanguageIndex]);

            charaData.Name = name.InnerText;

            foreach (XmlNode skin in node.SelectNodes("Skins/Skin"))
            {
                charaData.skins.Add(skin.InnerText);
            }

            newChara.commands = new();
            newChara.passiveSkill = new();
            XmlNode Skills = node.SelectSingleNode("Skills");
            foreach(XmlNode skill in Skills.SelectNodes("Skill"))
            {
                Type skl = Type.GetType(skill["class"].InnerText);
                Skill target = Activator.CreateInstance(skl) as Skill;
                target.name = skill[GameManager.gameData.Language[GameManager.gameData.LanguageIndex]].InnerText;
                var cmd = SetCommands(skill["command"].InnerText);

                if(cmd.Count > 0)
                {
                    newChara.commands[SetCommands(skill["command"].InnerText)] = target;
                }
                else
                {
                    newChara.passiveSkill.Add(target);
                }

            }

            newChara.SetType(node["AtkType"].InnerText, node["DefType"].InnerText);

            newChara.charaData = charaData;

            charactors[int.Parse(node["id"].InnerText)] = newChara;

            
        }
    }

    public List<KeyValues> SetCommands(string command)
    {
        List<KeyValues> ret = new();
        KeyValues kv;

        foreach(char c in command)
        {
            switch (c)
            {
                case 'A':
                    kv = KeyValues.Shot;
                    if(ret.Count > 0)
                    {
                        kv += (int)ret.Last();
                    }
                    ret.Add(kv);
                    break;
                case 'B':
                    kv = (KeyValues)KeyValues.Jump;
                    if (ret.Count > 0)
                    {
                        kv += (int)ret.Last();
                    }
                    ret.Add(kv);
                    break;
                case 'C':
                    kv = (KeyValues)KeyValues.Call;
                    if (ret.Count > 0)
                    {
                        kv += (int)ret.Last();
                    }
                    ret.Add(kv);
                    break;
                case '1':
                    ret.Add(KeyValues.DownLeft);
                    break;
                case '2':
                    ret.Add(KeyValues.Down);
                    break;
                case '3':
                    ret.Add(KeyValues.DownRight);
                    break;
                case '4':
                    ret.Add(KeyValues.Left);
                    break;
                case '6':
                    ret.Add(KeyValues.Right);
                    break;
                case '7':
                    ret.Add(KeyValues.UpLeft);
                    break;
                case '8':
                    ret.Add(KeyValues.Up);
                    break;
                case '9':
                    ret.Add(KeyValues.UpRight);
                    break;
            }
        }

        return ret;
    }
}
