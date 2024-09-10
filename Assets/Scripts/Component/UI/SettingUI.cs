using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : Menu
{
    public CustomDropDown resolutions;
    public CustomScrollBar masterVolume;
    public TMP_Text masterVolumeTxt;
    public CustomScrollBar musicVolume;
    public TMP_Text musicVolumeTxt;
    public CustomScrollBar effectVolume;
    public TMP_Text effectVolumeTxt;
    public CustomDropDown languages;

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
        Debug.Log("FirstFresh");
        resolutions.dropdown.value = GameManager.gameData.resolutionIndex;
        masterVolume.scrollbar.value = GameManager.gameData.masterVolume;
        effectVolume.scrollbar.value = GameManager.gameData.effectVolume;
        musicVolume.scrollbar.value = GameManager.gameData.musicVolume;
        languages.dropdown.value = GameManager.gameData.LanguageIndex;

        masterVolumeTxt.text = (GameManager.gameData.masterVolume * 100).ToString() + " %";
        effectVolumeTxt.text = (GameManager.gameData.effectVolume * 100).ToString() + " %";
        musicVolumeTxt.text = (GameManager.gameData.musicVolume * 100).ToString() + " %";

        isDataSet = true;
    }

    public void RefreshValue()
    {
        if (isDataSet)
        {
            
            GameManager.gameData.masterVolume = masterVolume.scrollbar.value;
            GameManager.gameData.effectVolume = effectVolume.scrollbar.value;
            GameManager.gameData.musicVolume = musicVolume.scrollbar.value;
            GameManager.gameData.LanguageIndex = languages.dropdown.value;

            masterVolumeTxt.text = (GameManager.gameData.masterVolume * 100).ToString() + " %";
            effectVolumeTxt.text = (GameManager.gameData.effectVolume * 100).ToString() + " %";
            musicVolumeTxt.text = (GameManager.gameData.musicVolume * 100).ToString() + " %";
        }
    }

    public override HoveringRectTransform FindIndexButton(int index)
    {
        switch (index)
        {
            case 0:
                return resolutions.GetComponent<HoveringRectTransform>();
            case 1:
                return masterVolume.GetComponent<HoveringRectTransform>();
            case 2:
                return musicVolume.GetComponent<HoveringRectTransform>();
            case 3:
                return effectVolume.GetComponent<HoveringRectTransform>();
            case 4:
                return languages.GetComponent<HoveringRectTransform>();
            case -1:
                return closeButton.GetComponent<HoveringRectTransform>();
            case 999:
                return confirmButton.GetComponent<HoveringRectTransform>();
            default:
                return null;
        }
    }
}
