using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SettingManager
{

    public struct SerializeGameData
    {
        // Resoluition Lists
        public List<Resolution> resolutionLists;
        // Current Resolution
        public Resolution resolution;
        // Current Rexolution List Index
        public int resolutionIndex;
        // is Full Screen
        public bool isFullScreen;

        // Volumes
        public float masterVolume;
        public float musicVolume;
        public float effectVolume;
        public float charactorVolume;

        public int FrameRate;

        public List<string> Language;
        public int LanguageIndex;
    }

    public SerializeGameData gameData;

    public void SetResolution()
    {
        Screen.SetResolution(gameData.resolution.width, gameData.resolution.height, gameData.isFullScreen);
    }

    public SerializeGameData SetFirstSetting()
    {
        SerializeGameData ret = new SerializeGameData();

        ret.masterVolume = 0.5f;
        ret.musicVolume = 0.5f;
        ret.effectVolume = 0.5f;
        ret.charactorVolume = 0.5f;
        ret.resolutionIndex = 0;
        ret.isFullScreen = true;
        ret.FrameRate = 60;

        ret.resolution = ret.resolutionLists[0];

        ret.LanguageIndex = 0;

        PlayerPrefs.SetInt("FirstRun", 1);

        gameData = ret;

        return gameData;
    }

    public void SaveSettingData()
    {
        PlayerPrefs.SetFloat("masterVolume", gameData.masterVolume);
        PlayerPrefs.SetFloat("musicVolume", gameData.musicVolume);
        PlayerPrefs.SetFloat("effectVolume", gameData.effectVolume);
        PlayerPrefs.SetFloat("charactorVolume", gameData.charactorVolume);
        PlayerPrefs.SetInt("FullScreen", gameData.isFullScreen ? 1 : 0);
        PlayerPrefs.SetInt("FrameRate", gameData.FrameRate);
        PlayerPrefs.SetInt("Language", gameData.LanguageIndex);

        PlayerPrefs.SetInt("resolution", gameData.resolutionIndex);
    }


    public SerializeGameData LoadSettingData()
    {
        gameData.masterVolume = PlayerPrefs.GetFloat("masterVolume");
        gameData.musicVolume = PlayerPrefs.GetFloat("musicVolume");
        gameData.effectVolume = PlayerPrefs.GetFloat("effectVolume");
        gameData.charactorVolume = PlayerPrefs.GetFloat("charactorVolume");
        gameData.resolutionIndex = PlayerPrefs.GetInt("resolution");
        gameData.isFullScreen = PlayerPrefs.GetInt("FullScreen") == 1;
        gameData.FrameRate = PlayerPrefs.GetInt("FrameRate");
        gameData.LanguageIndex = PlayerPrefs.GetInt("Language");

        gameData.resolution = gameData.resolutionLists[gameData.resolutionIndex];

        return gameData;
    }
    public void SetResolutionList()
    {
        gameData.resolutionLists = new List<Resolution>();
        Resolution resolution = new Resolution();
        // 1920 * 1080 resolution
        resolution.width = 1920; resolution.height = 1080;
        gameData.resolutionLists.Add(resolution);
    }

    public void SetLanguageList()
    {
        gameData.Language = new List<string>{ "Kr", "Eng", "Jp" };
        Debug.Log(gameData.Language.Count);

    }

    public void SettingChange(string name, float value)
    {
        switch (name)
        {
            case "masterVolume":
                gameData.masterVolume = value;
                break;
            case "musicVolume":
                gameData.musicVolume = value;
                break;
            case "effectVolume":
                gameData.effectVolume = value;
                break;
            case "charactorVolume":
                gameData.charactorVolume = value;
                break;
            case "resolution":
                gameData.resolutionIndex = (int)value;
                gameData.resolution = gameData.resolutionLists[gameData.resolutionIndex];
               break;
            case "fullScreen":
                gameData.isFullScreen = value == 1f;
                break;
            case "frameRate":
                gameData.FrameRate = (int)value;
                break;
        }

        SaveSettingData();
    }
}
