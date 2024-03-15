using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;
[Serializable]
public class ScriptManager
{
    public List<MapTrigText> mapTexts = new List<MapTrigText>();
    public List<MapScript> scripts = new List<MapScript>();

    string scriptXMLFile = "Scripts";

    XmlDocument text;


    // Road Text Script XML
    public void init()
    {
        text = GameManager.Resource.LoadXML(scriptXMLFile);
        // Initialize Trigger Texts
        MapTrigText mapTrigText = new MapTrigText();
        XmlNodeList nodes = text.SelectNodes("//mapID");
        TrigText trigs = new TrigText();
        foreach(XmlNode node in nodes)
        {
            XmlNode trigData = node.SelectSingleNode("trigID");


            // if not Same Trigger Id
            if (!trigData.Attributes["name"].Value.Equals(trigs.trigId))
            {
                // Save Previous Trigger Script Data
                if (trigs.trigId != null)
                {
                    mapTrigText.trigTexts.Add(trigs);
                }
                trigs = new TrigText();
                trigs.trigId = trigData.Attributes["name"].Value;
            }

            Script script = new Script();
            script.npcId = trigData["script"]["NPCID"].InnerText;
            script.script = trigData["script"]["kr"].InnerText;
            trigs.scripts.Add(script);

            // if not Same Map Id
            if (!node.Attributes["name"].Value.Equals(mapTrigText.mapId))
            {

                // Save Previous Map Script Data
                if (mapTrigText.mapId != null)
                {
                    mapTexts.Add(mapTrigText);
                }
                // Create new MapScript
                mapTrigText = new MapTrigText();
                mapTrigText.mapId = node.Attributes["name"].Value;
            }

        }
        
        mapTrigText.trigTexts.Add(trigs);
        mapTexts.Add(mapTrigText);

        // Script Data Testing
/*   
        foreach(var map in mapTexts)
        {
            foreach(var trig in map.trigTexts)
            {
                foreach(var script in trig.scripts)
                {
                    Debug.Log(script.script);
                }
                Debug.Log("EndTrig " + trig.trigId);
            }
            Debug.Log("EndMap " + map.mapId);
        }
*/
    }


    public MapTrigText getMapTrigTextData(string mapId)
    {
        MapTrigText ret = mapTexts.Find(item => item.mapId.Equals(mapId));
        return ret;
    }
}

// Use Trigger active Texts
[Serializable]
public class MapTrigText
{
    public string mapId;
    public List<TrigText> trigTexts = new List<TrigText>();
}

[Serializable]
public class TrigText
{
    public string trigId;
    public List<Script> scripts = new List<Script>();
}

[Serializable]
public class Script 
{
    public string npcId;
    public string script;
    public bool isAwait;
    public string nextTextTrig;
}

// Use General NPC Texts
[Serializable]
public class MapScript
{
    public string mapId;
    List<TrigScript> scripts = new List<TrigScript>();
}

[Serializable]
public class TrigScript
{
    public string trigId;
    List<NPCScript> scripts = new List<NPCScript>();
}

[Serializable]
public class NPCScript
{
    public string npcId;
    public string script;
}