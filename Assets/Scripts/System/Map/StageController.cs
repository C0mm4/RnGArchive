using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StageController
{

    public Charactor currentCharactor;
    public int currentIndex;

    public MapTrigText currentMapTrigTexts;

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
        GameManager.Progress.currentParty.Add(GameManager.CharaCon.charactors[10001002]);
    }

    public void LoadMap(string mapId)
    {
        GameObject go = GameManager.InstantiateAsync(mapId);
        GameManager.Progress.saveMapId = mapId;
        GameManager.CameraManager.background = go.GetComponent<Map>().bound;
        
        // Set Map Trigger Text Datas
        currentMapTrigTexts = GameManager.Script.getMapTrigTextData(mapId);
        Trigger[] triggers = go.GetComponentsInChildren<Trigger>();
        foreach (Trigger trig in triggers)
        {
            Debug.Log(trig.data.id);
            TrigText trigText = currentMapTrigTexts.trigTexts.Find(item => item.trigId.Equals(trig.data.id));
            trig.SetTriggerTextData(trigText);
        }

        // Set Map NPC Text Datas (add later)
    }


}
