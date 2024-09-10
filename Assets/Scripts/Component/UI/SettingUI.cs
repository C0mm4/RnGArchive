using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : Menu
{
    public Scrollbar masterVolume;
    public TMP_Text masterVolumeTxt;
    public Scrollbar musicVolume;
    public TMP_Text musicVolumeTxt;
    public Scrollbar effectVolume;
    public TMP_Text effectVolumeTxt;

    public bool isDataSet = false;

    public override void OnCreate()
    {
        base.OnCreate();

        FirstFreshValue();
    }

    public override void ConfirmAction()
    {
        GameManager.Setting.SaveSettingData();
    }

    public void FirstFreshValue()
    {
        masterVolume.value = GameManager.gameData.masterVolume;
        effectVolume.value = GameManager.gameData.effectVolume;
        musicVolume.value = GameManager.gameData.musicVolume;

        masterVolumeTxt.text = (GameManager.gameData.masterVolume * 100).ToString() + " %";
        effectVolumeTxt.text = (GameManager.gameData.effectVolume * 100).ToString() + " %";
        musicVolumeTxt.text = (GameManager.gameData.musicVolume * 100).ToString() + " %";

        isDataSet = true;
    }

    public void RefreshValue()
    {
        if (isDataSet)
        {
            GameManager.gameData.masterVolume = masterVolume.value;
            GameManager.gameData.effectVolume = effectVolume.value;
            GameManager.gameData.musicVolume = musicVolume.value;

            masterVolumeTxt.text = (GameManager.gameData.masterVolume * 100).ToString() + " %";
            effectVolumeTxt.text = (GameManager.gameData.effectVolume * 100).ToString() + " %";
            musicVolumeTxt.text = (GameManager.gameData.musicVolume * 100).ToString() + " %";
        }
    }
}
