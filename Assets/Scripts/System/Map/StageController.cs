using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class StageController
{

    public Charactor currentCharactor;
    public int currentIndex;

    public Map currentMap;

    public MapTrigText currentMapTrigTexts;
    public MapScript currentMapNPCTexts;

    public List<Trigger> currentMapTrigger;

    public List<NPC> currentNPCs;

    public bool isTapInput = false;
    public int changeSlot = 0;
    public float changeInputT;

    // Update is called once per frame
    public void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Tab))
        {

            if (GameManager.Progress.currentParty.Count > 1)
            {


                if (!GameManager.Player.GetComponent<PlayerController>().isHitState)
                {
                    changeInputT = Time.time;
                    isTapInput = true;


                    GameManager.UIManager.inGameUI.charas[changeSlot].GetComponent<CharaSlotUI>().DisableChangeSlot();
                    changeSlot++;
                    changeSlot %= GameManager.Progress.currentParty.Count;

                    GameManager.UIManager.inGameUI.charas[changeSlot].GetComponent<CharaSlotUI>().EnableChangeSlot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.UIManager.MapToggle();
        }

        if (isTapInput)
        {
            if(Time.time - changeInputT >= 1f)
            {
                GameManager.UIManager.inGameUI.charas[changeSlot].GetComponent<CharaSlotUI>().DisableChangeSlot();
                if (changeSlot != 0)
                {
                    GameManager.CharactorChange(changeSlot);
                    changeSlot = 0;
                }

                isTapInput = false;
            }
        }
    }

    public void LoadMap(string mapId)
    {

        GameObject go = GameManager.InstantiateAsync(mapId);
        currentMap = go.GetComponent<Map>();
        GameManager.Progress.saveMapId = mapId;
        GameManager.CameraManager.background = go.GetComponent<Map>().bound;
        
        GameManager.Instance.currentMapObj = go;

        // Set Map Trigger Text Datas
        currentMapTrigTexts = GameManager.Script.getMapTrigTextData(mapId);
        Trigger[] triggers = go.GetComponentsInChildren<Trigger>();
        currentMapTrigger = new();
        foreach (Trigger trig in triggers)
        {
            if (!trig.data.id.Equals("SpawnTrigger"))
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


        // Set Doors Activate Datas
        var doors = currentMap.GetComponentsInChildren<Door>().ToList();
        foreach(Door door in doors)
        {
            if (GameManager.Progress.openDoors.Contains(door.id))
            {
                door.isActivate = true;
            }
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

    public void DoorActivate(string id)
    {
        GameManager.Progress.openDoors.Add(id);
        var doors = currentMap.GetComponentsInChildren<Door>().ToList();
        Door targetDoor = doors.Find(item => item.id == id);
        if(targetDoor != null)
        {
            targetDoor.isActivate = true;
        }
    }

    public void DoorDeActivate(string id)
    {
        if (GameManager.Progress.openDoors.Contains(id))
        {
            GameManager.Progress.openDoors.Remove(id);
        }

        var doors = currentMap.GetComponentsInChildren<Door>().ToList();
        Door targetDoor = doors.Find(item => item.id == id);
        if (targetDoor != null)
        {
            targetDoor.isActivate = false;
        }
    }
}
