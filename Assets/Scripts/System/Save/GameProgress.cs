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
    public List<Charactor> openCharactors;
    public Dictionary<string, TriggerData> activeTrigs;


}
