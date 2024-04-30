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
        GameManager.Progress.isActiveSupport = false;
        GameManager.Progress.openCharactors = new();
        GameManager.Progress.currentParty = new();
        GameManager.Stage.SetInitializeParty();
        GameManager.Progress.currentSupporterId = 10002001;

        GameManager.Progress.AddNewCharas(10001001);
    }

    public void SaveGameprogress(SaveObj savePoint)
    {
        GameProgress progress = GameManager.Progress;
        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Create))
        {
            BinaryWriter writer = new BinaryWriter(fileStream);
            // Save SavePoints and MapData
            writer.Write(progress.saveMapId);
            Debug.Log(progress.saveMapId);
            writer.Write(savePoint.transform.position.x);
            Debug.Log(savePoint.transform.position.x);
            writer.Write(savePoint.transform.position.y);
            Debug.Log(savePoint.transform.position.y);

            // Save Current Charactor Id, Skill Activation
            writer.Write(progress.currentCharactorId);
            Debug.Log(progress.currentCharactorId);
            writer.Write(progress.currentSupporterId);
            Debug.Log(progress.currentSupporterId);
            writer.Write(progress.isActiveSkill ? true : false);
            Debug.Log(progress.isActiveSkill);
            writer.Write(progress.isActiveSupport ? true : false);
            Debug.Log(progress.isActiveSkill);

            // Save Open Charactor Counts
            writer.Write(progress.openCharactors.Count);
            Debug.Log(progress.openCharactors.Count);
            // Save Charactors Data
            foreach(int chara in progress.openCharactors)
            {
                Charactor targetChara = progress.charaDatas[chara].charactor;
                // Write Charactor Id
                writer.Write(targetChara.charaData.id);
                Debug.Log(targetChara.charaData.id);
                writer.Write(targetChara.skills.Count);
                Debug.Log(targetChara.skills.Count);
                // Save Charactors Skill Ammos
                foreach(Skill skill in targetChara.skills)
                {
                    // Write Skill data
                    writer.Write(skill.name);
                    Debug.Log(skill.name);
                    writer.Write(skill.maxAmmo);
                    Debug.Log(skill.maxAmmo);
                    writer.Write(skill.currentAmmo);
                    Debug.Log(skill.currentAmmo);
                }
                // Save Charactor Status
                writer.Write(targetChara.charaData.maxHP);
                Debug.Log(targetChara.charaData.maxHP);
                writer.Write(targetChara.charaData.currentHP);
                Debug.Log(targetChara.charaData.maxHP);
                writer.Write(targetChara.charaData.currentSkin);
                Debug.Log(targetChara.charaData.currentSkin);
            }

            // Save Open Supporter Counts
            writer.Write(progress.openSupporeters.Count);
            Debug.Log(progress.openSupporeters.Count);
            // Save Supporter Data
            foreach(Supporter supporter in progress.openSupporeters)
            {
                // Write Supporter Id
                writer.Write(supporter.data.id);
                Debug.Log(supporter.data.id);
                writer.Write(supporter.data.maxAmmo);
                Debug.Log(supporter.data.maxAmmo);
                writer.Write(supporter.data.currentAmmo);
                Debug.Log(supporter.data.currentAmmo);
            }

            // Save Current Party size
            writer.Write(progress.currentParty.Count);
            Debug.Log(progress.currentParty.Count);
            // Save Party Data
            foreach(Charactor chara in progress.currentParty)
            {
                writer.Write(chara.charaData.id);
                Debug.Log(chara.charaData.id);
            }

            // Save Trigger Datas
            writer.Write(progress.activeTrigs.Count);
            Debug.Log(progress.activeTrigs.Count);
            foreach(TriggerData trig in progress.activeTrigs.Values)
            {
                writer.Write(trig.id);
                Debug.Log(trig.id);
            }

            // Save Door Datas
            writer.Write(progress.openDoors.Count);
            Debug.Log(progress.openDoors.Count);
            foreach(string doorId in progress.openDoors)
            {
                writer.Write(doorId);
                Debug.Log(doorId);
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
                float x, y;
                x = reader.ReadSingle(); y = reader.ReadSingle();
                saveProgress.saveP = new Vector3(x, y);
                Debug.Log(x);
                Debug.Log(y);
                saveProgress.currentCharactorId = reader.ReadInt32();
                Debug.Log(saveProgress.currentCharactorId);
                saveProgress.currentSupporterId = reader.ReadInt32();
                Debug.Log(saveProgress.currentSupporterId);
                saveProgress.isActiveSkill = reader.ReadBoolean() == true;
                Debug.Log(saveProgress.isActiveSkill);
                saveProgress.isActiveSupport = reader.ReadBoolean() == true;
                Debug.Log(saveProgress.isActiveSupport);
                // Load Charactor Datas
                int charaCnt = reader.ReadInt32();
                Debug.Log(charaCnt);
                for (int i = 0; i < charaCnt; i++)
                {
                    int targetCharaId = reader.ReadInt32();
                    Debug.Log(targetCharaId);
                    Charactor targetChara = GameManager.CharaCon.charactors[targetCharaId];
                    
                    int skillCnt = reader.ReadInt32();
                    Debug.Log(skillCnt);
                    for (int j = 0; j < skillCnt; j++)
                    {
                        string name = reader.ReadString();
                        Debug.Log(name);
                        int skill = targetChara.skills.FindIndex(item => item.name == name);
                        targetChara.skills[skill].maxAmmo = reader.ReadInt32();
                        Debug.Log(targetChara.skills[skill].maxAmmo);
                        targetChara.skills[skill].currentAmmo = reader.ReadInt32();
                        Debug.Log(targetChara.skills[skill].currentAmmo);

                    }

                    targetChara.charaData.maxHP = reader.ReadInt32();
                    Debug.Log(targetChara.charaData.maxHP);
                    targetChara.charaData.currentHP = reader.ReadInt32();
                    Debug.Log(targetChara.charaData.currentHP);
                    targetChara.charaData.currentSkin = reader.ReadInt32();
                    Debug.Log(targetChara.charaData.currentSkin);

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
                Debug.Log(supportCnt);
                for (int i = 0; i < supportCnt; i++)
                {
                    int supportId = reader.ReadInt32();
                    GameManager.CharaCon.supporters[supportId].data.maxAmmo = reader.ReadInt32();
                    Debug.Log(saveProgress.isActiveSupport);
                    GameManager.CharaCon.supporters[supportId].data.currentAmmo = reader.ReadInt32();
                    Debug.Log(saveProgress.isActiveSupport);
                }

                // Load Party Data
                int partySize = reader.ReadInt32();
                Debug.Log(partySize);
                for (int i = 0; i <  partySize; i++)
                {
                    saveProgress.currentParty.Add(GameManager.CharaCon.charactors[reader.ReadInt32()]);
                    Debug.Log(saveProgress.currentParty);
                }

                // Load Triggers
                int cnt = reader.ReadInt32();
                Debug.Log(cnt);
                for(int i = 0; i < cnt; i++)
                {
                    string id = reader.ReadString();
                    Debug.Log(id);
                    TriggerData trigData = new TriggerData(id, true);
                    saveProgress.activeTrigs[id] = trigData;
                }

                // Load Door Activate Datas
                int doorCnt = reader.ReadInt32();
                Debug.Log(doorCnt);
                for (int i = 0; i < doorCnt; i++)
                {
                    string doorId = reader.ReadString();
                    saveProgress.openDoors.Add(doorId);
                    Debug.Log(doorId);
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
