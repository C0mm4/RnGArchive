using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class StageController
{

    public Charactor currentCharactor;
    public int currentIndex;

    public MapTrigText currentMapTrigTexts;
    public MapScript currentMapNPCTexts;

    public List<Trigger> currentMapTrigger;

    public List<NPC> currentNPCs;

    // Update is called once per frame
    public void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.Progress.currentParty.Count > 1)
            {
                currentIndex++;
                currentIndex %= GameManager.Progress.currentParty.Count;

                GameManager.NextCharactor();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.UIManager.MapToggle();
        }
    }

    public void SetInitializeParty()
    {
        GameManager.Progress.currentParty.Add(GameManager.CharaCon.charactors[10001001]);
    }

    public void LoadMap(string mapId)
    {

        GameObject go = GameManager.InstantiateAsync(mapId);
        GameManager.Progress.saveMapId = mapId;
        GameManager.CameraManager.background = go.GetComponent<Map>().bound;
        
        GameManager.Instance.currentMapObj = go;

        // Set Map Trigger Text Datas
        currentMapTrigTexts = GameManager.Script.getMapTrigTextData(mapId);
        Trigger[] triggers = go.GetComponentsInChildren<Trigger>();
        currentMapTrigger = new();
        foreach (Trigger trig in triggers)
        {
            Debug.Log(trig.data.id);
            if (!trig.data.id.Equals("Spawnger"))
            {
                TrigText trigText = currentMapTrigTexts.trigTexts.Find(item => item.trigId.Equals(trig.data.id));
                trig.SetTriggerTextData(trigText);
                if (!trigText.scripts[0].startNPCId.Equals(""))
                {
                    trig.data.startNPCId = trigText.scripts[0].startNPCId;
                }
            }
            currentMapTrigger.Add(trig);
        }

        // Set Map NPC Text Datas (add later)
        currentMapNPCTexts = GameManager.Script.getMapScriptsData(mapId);
        if(currentMapNPCTexts != null)
        {
            var scripts = currentMapNPCTexts.scripts;
            currentNPCs = go.GetComponentsInChildren<NPC>().ToList();
            foreach (TrigScript script in scripts)
            {
                foreach (NPC npc in currentNPCs)
                {
                    npc.AddScripts(script);
                }
            }

        }

        // SetNPC Text in Load
        if(currentNPCs != null)
            foreach(NPC npc in currentNPCs)
            {
                npc.SetScripts();
            }
    }

    public void NPCScriptSet(NPC npc)
    {
        if (currentMapNPCTexts != null)
        {
            foreach(TrigScript script in currentMapNPCTexts.scripts)
            {
                npc.AddScripts(script);
            }
        }
    }
}
