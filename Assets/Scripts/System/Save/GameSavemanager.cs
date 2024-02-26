using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameSavemanager
{
    private string saveFilePath;
    public GameProgress progress;

    public void initialize()
    {
        saveFilePath = "SaveData.dat";

    }

    public void NewGame()
    {
        progress = new GameProgress();
        GameManager.Trigger.activeTriggerLists = new Dictionary<string, TriggerData>();
    }

    public void SaveGameprogress()
    {
        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Create))
        {
            BinaryWriter writer = new BinaryWriter(fileStream);
            // Save Trigger Datas
            writer.Write(GameManager.Trigger.activeTriggerLists.Count);
            foreach(TriggerData trig in GameManager.Trigger.activeTriggerLists.Values)
            {
                writer.Write(trig.id);
                writer.Write(trig.isActivate);
            }
        }
    }
    public void LoadProgress()
    {
        if(File.Exists(saveFilePath))
        {
            GameProgress saveProgress = new GameProgress();
            GameManager.Trigger.activeTriggerLists = new();
            using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(fileStream);
                // Load Triggers
                int cnt = reader.ReadInt32();
                Debug.Log(cnt);
                for(int i = 0; i < cnt; i++)
                {
                    string id = reader.ReadString();
                    bool isActive = reader.ReadBoolean();
                    TriggerData trigData = new TriggerData(id, isActive);
                    GameManager.Trigger.ActiveTrigger(trigData);
                }
            }
            progress = saveProgress;
        }
        else
        {
            progress = new GameProgress();
        }
    }

}
