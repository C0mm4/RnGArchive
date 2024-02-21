using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ScriptManager
{
    public Dictionary<int, List<string>> scripts;

    string scriptXMLFile = "ScriptXML";

    XmlDocument text;

    // Road Text Script XML
    public void init()
    {
//        text = GameManager.Resource.LoadXML(scriptXMLFile);
//        scripts = new Dictionary<int, List<string>>();
    }

    // Return Text Data by Index
    public List<string> getTxtData(int index)
    {
        List<string> ret = new List<string>();

        XmlNode node = text.SelectSingleNode($"/ScriptData/Script[id='{index}']");
        if (node != null)
        {
            XmlNodeList textNode = node.SelectNodes("Text");
            foreach (XmlNode text in textNode)
            {
                ret.Add(text.InnerText);
            }

            return ret;
        }
        else
        {
            Debug.Log($"Can't Find Script about : {index}");
            return null;
        }
    }
}
