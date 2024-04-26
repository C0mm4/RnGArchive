using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.ExceptionServices;

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
        GameManager.Progress.isActiveSupport = false;
        GameManager.Progress.openCharactors = new();
        GameManager.Progress.currentParty = new();
        GameManager.Stage.SetInitializeParty();
        GameManager.Progress.currentSupporterId = 10002001;
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

            // Save Current Charactor Id, Skill Activation
            writer.Write(GameManager.Progress.currentCharactorId);
            writer.Write(GameManager.Progress.currentSupporterId);
            writer.Write(GameManager.Progress.isActiveSkill ? true : false);
            writer.Write(GameManager.Progress.isActiveSupport ? true : false);

            // Save Open Charactor Counts
            writer.Write(GameManager.Progress.openCharactors.Count);
            // Save Charactors Data
            foreach(Charactor chara in GameManager.Progress.openCharactors)
            {
                // Write Charactor Id
                writer.Write(chara.charaData.id);
                writer.Write(chara.skills.Count);
                // Save Charactors Skill Ammos
                foreach(Skill skill in chara.skills)
                {
                    // Write Skill data
                    writer.Write(skill.name);
                    writer.Write(skill.maxAmmo);
                    writer.Write(skill.currentAmmo);
                }
                // Save Charactor Status
                writer.Write(chara.charaData.maxHP);
                writer.Write(chara.charaData.currentHP);
                writer.Write(chara.charaData.currentSkin);
            }

            // Save Open Supporter Counts
            writer.Write(GameManager.Progress.openSupporeters.Count);
            // Save Supporter Data
            foreach(Supporter supporter in GameManager.Progress.openSupporeters)
            {
                // Write Supporter Id
                writer.Write(supporter.data.id);
                writer.Write(supporter.data.maxAmmo);
                writer.Write(supporter.data.currentAmmo);
            }

            // Save Current Party size
            writer.Write(GameManager.Progress.currentParty.Count);
            // Save Party Data
            foreach(Charactor chara in GameManager.Progress.currentParty)
            {
                writer.Write(chara.charaData.id);
            }

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
                saveProgress.currentSupporterId = reader.ReadInt32();
                saveProgress.isActiveSkill = reader.ReadInt32() == 1;
                saveProgress.isActiveSupport = reader.ReadInt32() == 1;

                // Load Charactor Datas
                int charaCnt = reader.ReadInt32();
                for(int i = 0; i < charaCnt; i++)
                {
                    int targetCharaId = reader.ReadInt32();
                    Charactor targetChara = GameManager.CharaCon.charactors[targetCharaId];
                    string name = reader.ReadString();
                    
                    int skillCnt = reader.ReadInt32();
                    for(int j = 0; j < skillCnt; j++)
                    {
                        int skill = targetChara.skills.FindIndex(item => item.name == name);
                        targetChara.skills[skill].maxAmmo = reader.ReadInt32();
                        targetChara.skills[skill].currentAmmo = reader.ReadInt32();

                    }

                    targetChara.charaData.maxHP = reader.ReadInt32();
                    targetChara.charaData.currentHP = reader.ReadInt32();
                    targetChara.charaData.currentSkin = reader.ReadInt32();

                    // OverWrite Charactor Datas
                    GameManager.CharaCon.charactors[targetCharaId] = targetChara;
                }

                // Load Supporter Datas
                int supportCnt = reader.ReadInt32();
                for(int i = 0; i < supportCnt; i++)
                {
                    int supportId = reader.ReadInt32();
                    GameManager.CharaCon.supporters[supportId].data.maxAmmo = reader.ReadInt32();
                    GameManager.CharaCon.supporters[supportId].data.currentAmmo = reader.ReadInt32();
                }

                // Load Party Data
                int partySize = reader.ReadInt32();
                for(int i = 0; i <  partySize; i++)
                {
                    GameManager.Progress.currentParty.Add(GameManager.CharaCon.charactors[reader.ReadInt32()]);
                }

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
