using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]   
public class GameProgress 
{
    public string saveMapId;
    public Vector3 saveP;
    public string currentCharactorId;
    public string currentSupporterId;
    public bool isActiveSkill;

    public bool _isActiveSupport;
    public bool isActiveSupport { get { return isActiveSkill; } set { _isActiveSupport = value; } }
    public List<string> openCharactors = new();
    public Dictionary<string, CharactorProgress> charaDatas = new();
    public List<Supporter> openSupporeters = new();
    public List<Charactor> currentParty = new();
    public List<string> openDoors = new();


    public Dictionary<string, TriggerData> activeTrigs;

    [Serializable]
    public struct CharactorProgress
    {
        public Charactor charactor;
        public bool isOpen;

    }

    public void AddNewCharas(string charactorId)
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

    public void AbleChara(string charactorId)
    {
        CharactorProgress charaPros = new CharactorProgress();
        charaPros.charactor = GameManager.CharaCon.charactors[charactorId];
        charaPros.isOpen = true;
        charaDatas[charactorId] = charaPros;
    }

    public void DisableChara(string charactorId)
    {
        CharactorProgress charaPros = charaDatas[charactorId];
        charaPros.isOpen = false;
        charaDatas[charactorId] = charaPros;
    }

    public void InsertCharaInParty(string charactorId)
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

    public void DeleteCharaInParty(string charactorId)
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
        string id = progress.charactor.charaData.id;
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
            openSupporeters.Add(GameManager.CharaCon.supporters[specialId]);
            currentSupporterId = specialId;
        }
    }

    public void AddNewSpecial(string specialId)
    {
        if (!openSupporeters.Exists(item => item.data.id.Equals(specialId)))
        {
            openSupporeters.Add(GameManager.CharaCon.supporters[specialId]);
        }
    }
}
