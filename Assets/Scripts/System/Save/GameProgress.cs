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
    public List<Charactor> openCharactors;
    public List<Supporter> openSupporeters;
    public List<Charactor> currentParty;


    public Dictionary<string, TriggerData> activeTrigs;
}
