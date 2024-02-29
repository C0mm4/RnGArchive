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


    public void SetResolution()
    {
        Screen.SetResolution(GameManager.gameData.resolution.width, GameManager.gameData.resolution.height, GameManager.gameData.isFullScreen);
    }

    public void SetFirstSetting()
    {
        GameManager.gameData.masterVolume = 0.5f;
        GameManager.gameData.musicVolume = 0.5f;
        GameManager.gameData.effectVolume = 0.5f;
        GameManager.gameData.charactorVolume = 0.5f;
        GameManager.gameData.resolutionIndex = 0;
        GameManager.gameData.isFullScreen = true;
        GameManager.gameData.FrameRate = 60;

        GameManager.gameData.resolution = GameManager.gameData.resolutionLists[0];

        GameManager.gameData.LanguageIndex = 0;

        PlayerPrefs.SetInt("FirstRun", 1);

    }

    public void SaveSettingData()
    {
        PlayerPrefs.SetFloat("masterVolume", GameManager.gameData.masterVolume);
        PlayerPrefs.SetFloat("musicVolume", GameManager.gameData.musicVolume);
        PlayerPrefs.SetFloat("effectVolume", GameManager.gameData.effectVolume);
        PlayerPrefs.SetFloat("charactorVolume", GameManager.gameData.charactorVolume);
        PlayerPrefs.SetInt("FullScreen", GameManager.gameData.isFullScreen ? 1 : 0);
        PlayerPrefs.SetInt("FrameRate", GameManager.gameData.FrameRate);
        PlayerPrefs.SetInt("Language", GameManager.gameData.LanguageIndex);

        PlayerPrefs.SetInt("resolution", GameManager.gameData.resolutionIndex);
    }


    public void LoadSettingData()
    {
        GameManager.gameData.masterVolume = PlayerPrefs.GetFloat("masterVolume");
        GameManager.gameData.musicVolume = PlayerPrefs.GetFloat("musicVolume");
        GameManager.gameData.effectVolume = PlayerPrefs.GetFloat("effectVolume");
        GameManager.gameData.charactorVolume = PlayerPrefs.GetFloat("charactorVolume");
        GameManager.gameData.resolutionIndex = PlayerPrefs.GetInt("resolution");
        GameManager.gameData.isFullScreen = PlayerPrefs.GetInt("FullScreen") == 1;
        GameManager.gameData.FrameRate = PlayerPrefs.GetInt("FrameRate");
        GameManager.gameData.LanguageIndex = PlayerPrefs.GetInt("Language");

        GameManager.gameData.resolution = GameManager.gameData.resolutionLists[GameManager.gameData.resolutionIndex];

    }
    public void SetResolutionList()
    {
        GameManager.gameData.resolutionLists = new List<Resolution>();
        Resolution resolution = new Resolution();
        // 1920 * 1080 resolution
        resolution.width = 1920; resolution.height = 1080;
        GameManager.gameData.resolutionLists.Add(resolution);
    }

    public void SetLanguageList()
    {
        GameManager.gameData.Language = new List<string>{ "Kr", "Eng", "Jp" };

    }

    public void SettingChange(string name, float value)
    {
        switch (name)
        {
            case "masterVolume":
                GameManager.gameData.masterVolume = value;
                break;
            case "musicVolume":
                GameManager.gameData.musicVolume = value;
                break;
            case "effectVolume":
                GameManager.gameData.effectVolume = value;
                break;
            case "charactorVolume":
                GameManager.gameData.charactorVolume = value;
                break;
            case "resolution":
                GameManager.gameData.resolutionIndex = (int)value;
                GameManager.gameData.resolution = GameManager.gameData.resolutionLists[GameManager.gameData.resolutionIndex];
               break;
            case "fullScreen":
                GameManager.gameData.isFullScreen = value == 1f;
                break;
            case "frameRate":
                GameManager.gameData.FrameRate = (int)value;
                break;
        }

        SaveSettingData();
    }
}
