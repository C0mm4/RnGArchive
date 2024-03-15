using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameSavemanager
{
    private string saveFilePath;


    public void initialize()
    {
        saveFilePath = "SaveData.dat";
    }

    public void NewGame()
    {
        GameManager.Progress = new GameProgress();
        GameManager.Progress.activeTrigs = new Dictionary<string, TriggerData>();
        GameManager.Progress.isActiveSkill = false;
        GameManager.Progress.openCharactors = new();
        GameManager.Progress.currentParty = new();
        GameManager.Stage.SetInitializeParty();
    }

    public void SaveGameprogress(SaveObj savePoint)
    {
        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Create))
        {
            BinaryWriter writer = new BinaryWriter(fileStream);
            // Save SavePoints and MapData
            writer.Write(GameManager.Progress.saveMapId);
            Debug.Log(GameManager.Progress.saveMapId);
            writer.Write(savePoint.transform.position.x);
            Debug.Log(savePoint.transform.position.x);
            writer.Write(savePoint.transform.position.y);
            Debug.Log(savePoint.transform.position.y);
            writer.Write(GameManager.Progress.currentCharactorId);
            Debug.Log(GameManager.Progress.currentCharactorId);
            writer.Write(GameManager.Progress.isActiveSkill ? 1 : 0);

            // Save Trigger Datas
            writer.Write(GameManager.Progress.activeTrigs.Count);
            foreach(TriggerData trig in GameManager.Progress.activeTrigs.Values)
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
            Dictionary<string, TriggerData> trigs = new();
            saveProgress.activeTrigs = trigs;

            using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(fileStream);

                saveProgress.saveMapId= reader.ReadString();
                Debug.Log(saveProgress.saveMapId);
                saveProgress.saveP = new Vector3(reader.ReadSingle(), reader.ReadSingle());
                Debug.Log(saveProgress.saveP);
                saveProgress.currentCharactorId = reader.ReadInt32();
                Debug.Log(saveProgress.currentCharactorId);
                saveProgress.isActiveSkill = reader.ReadInt32() == 1;

                // Load Triggers
                int cnt = reader.ReadInt32();
                Debug.Log(cnt);
                for(int i = 0; i < cnt; i++)
                {
                    string id = reader.ReadString();
                    bool isActive = reader.ReadBoolean();
                    TriggerData trigData = new TriggerData(id, isActive);
                    saveProgress.activeTrigs[id] = trigData;
                }
            }
            GameManager.Progress = saveProgress;
        }
        else
        {
            GameManager.Progress = new GameProgress();
        }
    }

}
