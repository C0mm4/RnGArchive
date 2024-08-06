using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]   
public class GameProgress 
{
    public string saveMapId;
    public Vector3 saveP;
    public int currentCharactorId;
    public int currentSupporterId;
    public bool isActiveSkill;
    public bool isActiveSupport;
    public List<int> openCharactors = new();
    public Dictionary<int, CharactorProgress> charaDatas = new();
    public List<Supporter> openSupporeters = new();
    public List<Charactor> currentParty = new();
    public List<string> openDoors = new();


    public Dictionary<string, TriggerData> activeTrigs;

    public struct CharactorProgress
    {
        public Charactor charactor;
        public bool isOpen;

    }

    public void AddNewCharas(int charactorId)
    {
        CharactorProgress charaPros = new CharactorProgress();
        charaPros.charactor = GameManager.CharaCon.charactors[charactorId];
        charaPros.isOpen = true;

        if (!openCharactors.Contains(charactorId))
        {
            openCharactors.Add(charactorId);
        }
        if (!charaDatas.ContainsKey(charactorId))
        {
            charaDatas[charactorId] = charaPros;
        }
    }

    public void AbleChara(int charactorId)
    {
        CharactorProgress charaPros = charaDatas[charactorId];
        charaPros.isOpen = true;
        charaDatas[charactorId] = charaPros;
    }

    public void DisableChara(int charactorId)
    {
        CharactorProgress charaPros = charaDatas[charactorId];
        charaPros.isOpen = false;
        charaDatas[charactorId] = charaPros;
    }

    public void InsertCharaInParty(int charactorId)
    {
        if (!openCharactors.Contains(charactorId))
        {
            AddNewCharas(charactorId);
        }
        if(currentParty.Count == 0)
        {
            currentCharactorId = charactorId;
        }
        currentParty.Add(GameManager.Progress.charaDatas[charactorId].charactor);
    }

    public void DeleteCharaInParty(int charactorId)
    {
        if (currentParty.Contains(GameManager.Progress.charaDatas[charactorId].charactor))
        {
            int index = currentParty.FindIndex(item => item.charaData.id == charactorId);
            if(index > 0)
            {
                currentCharactorId = currentParty[index - 1].charaData.id;
            }
            currentParty.RemoveAt(index);
        }
    }

    public void AddNewCharas(CharactorProgress progress)
    {
        int id = progress.charactor.charaData.id;
        if(!openCharactors.Contains(id))
        {
            openCharactors.Add(id);
        }
        if (!charaDatas.ContainsKey(id))
        {
            charaDatas[id] = progress;
        }
    }

    public void SetNewSpecial(string specialId)
    {
        if (!openSupporeters.Exists(item => item.data.id.Equals(specialId)))
        {
            openSupporeters.Add(GameManager.CharaCon.supporters[int.Parse(specialId)]);
            currentSupporterId = int.Parse(specialId);
        }
    }

    public void AddNewSpecial(string specialId)
    {
        if (!openSupporeters.Exists(item => item.data.id.Equals(specialId)))
        {
            openSupporeters.Add(GameManager.CharaCon.supporters[int.Parse(specialId)]);
        }
    }
}
