using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.ExceptionServices;
using UnityEditor;

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
        GameManager.Progress.isActiveSkill = true;
        GameManager.Progress.isActiveSupport = true;
        GameManager.Progress.openCharactors = new();
        GameManager.Progress.currentParty = new();
//        GameManager.Stage.SetInitializeParty();
        GameManager.Progress.currentSupporterId = 10002001;
        GameManager.Progress.currentCharactorId = 10001000;

//        GameManager.Progress.AddNewCharas(10001000);
        GameManager.Progress.InsertCharaInParty(10001000);
    }

    public void SaveGameprogress(Transform savePoint)
    {
        GameProgress progress = GameManager.Progress;
        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Create))
        {
            BinaryWriter writer = new BinaryWriter(fileStream);
            // Save SavePoints and MapData
            writer.Write(progress.saveMapId);
            writer.Write(savePoint.transform.position.x);
            writer.Write(savePoint.transform.position.y);

            // Save Current Charactor Id, Skill Activation
            writer.Write(progress.currentCharactorId);
            writer.Write(progress.currentSupporterId);
            writer.Write(progress.isActiveSkill ? true : false);
            writer.Write(progress.isActiveSupport ? true : false);

            // Save Open Charactor Counts
            writer.Write(progress.openCharactors.Count);
            // Save Charactors Data
            foreach(int chara in progress.openCharactors)
            {
                Charactor targetChara = progress.charaDatas[chara].charactor;
                // Write Charactor Id
                writer.Write(targetChara.charaData.id);

                // Save Charactor Status
                writer.Write(targetChara.charaData.maxHP);
                writer.Write(targetChara.charaData.currentHP);
                writer.Write(targetChara.charaData.currentSkin);
            }

            // Save Open Supporter Counts
            writer.Write(progress.openSupporeters.Count);
            // Save Supporter Data
            foreach(Supporter supporter in progress.openSupporeters)
            {
                // Write Supporter Id
                writer.Write(supporter.data.id);
            }

            // Save Current Party size
            writer.Write(progress.currentParty.Count);
            // Save Party Data
            foreach(Charactor chara in progress.currentParty)
            {
                writer.Write(chara.charaData.id);
            }

            // Save Trigger Datas
            writer.Write(progress.activeTrigs.Count);
            foreach(TriggerData trig in progress.activeTrigs.Values)
            {
                writer.Write(trig.id);
            }

            // Save Door Datas
            writer.Write(progress.openDoors.Count);
            foreach(string doorId in progress.openDoors)
            {
                writer.Write(doorId);
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

                var saveMapId = reader.ReadString();
                saveProgress.saveMapId= saveMapId;
                float x, y;
                x = reader.ReadSingle(); y = reader.ReadSingle();
                saveProgress.saveP = new Vector3(x, y);

                var currentCharaId = reader.ReadInt32();
                saveProgress.currentCharactorId = currentCharaId;
                var currentSupporterId = reader.ReadInt32();
                saveProgress.currentSupporterId = currentSupporterId;
                var skill = reader.ReadBoolean();
                saveProgress.isActiveSkill = skill == true;
                var spe = reader.ReadBoolean();
                saveProgress.isActiveSupport = spe == true;
                // Load Charactor Datas
                int charaCnt = reader.ReadInt32();
                for (int i = 0; i < charaCnt; i++)
                {
                    int targetCharaId = reader.ReadInt32();
                    Charactor targetChara = GameManager.CharaCon.charactors[targetCharaId];
                    

                    targetChara.charaData.maxHP = reader.ReadInt32();
                    targetChara.charaData.currentHP = reader.ReadInt32();
                    targetChara.charaData.currentSkin = reader.ReadInt32();

                    // OverWrite Charactor Datas
                    //                    GameManager.CharaCon.charactors[targetCharaId] = targetChara;
                    GameProgress.CharactorProgress charaPros = new();
                    charaPros.charactor = targetChara;
                    charaPros.isOpen = true;
                    
                    saveProgress.AddNewCharas(charaPros);
                }

                // Data Input Progress and fixed Load Code
                // Load Supporter Datas
                int supportCnt = reader.ReadInt32();
                for (int i = 0; i < supportCnt; i++)
                {
                    string supportId = reader.ReadString();
                    saveProgress.AddNewSpecial(supportId);
                }

                // Load Party Data
                int partySize = reader.ReadInt32();
                for (int i = 0; i <  partySize; i++)
                {
                    int partyId = reader.ReadInt32();
                    saveProgress.currentParty.Add(GameManager.CharaCon.charactors[partyId]);
                }

                // Load Triggers
                int cnt = reader.ReadInt32();
                for(int i = 0; i < cnt; i++)
                {
                    string id = reader.ReadString();
                    TriggerData trigData = new TriggerData(id, true);
                    saveProgress.activeTrigs[id] = trigData;
                }

                // Load Door Activate Datas
                int doorCnt = reader.ReadInt32();
                for (int i = 0; i < doorCnt; i++)
                {
                    string doorId = reader.ReadString();
                    saveProgress.openDoors.Add(doorId);
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
