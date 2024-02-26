using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSavemanager
{
    private string saveFilePath;

    public void initialize()
    {
        saveFilePath = "";
    }

    public void SaveGameprogress(GameProgress progress)
    {

    }
    public GameProgress LoadProgress()
    {
        if(File.Exists(saveFilePath))
        {
            return null;
        }
        else
        {
            return null;
        }
    }

}
