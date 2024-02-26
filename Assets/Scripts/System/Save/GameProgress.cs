using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]   
public class GameProgress 
{
    public List<Trigger> triggers = new List<Trigger>();
    public Vector3 saveP;
    public List<Charactor> openCharactors;

    public void WriteToBinary(BinaryWriter writer)
    {

    }

    public static GameProgress ReadFromBinary(BinaryReader reader)
    {
        GameProgress progress = new GameProgress();



        return progress;
    }
}
