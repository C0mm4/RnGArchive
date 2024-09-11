using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : Menu
{
    public HoveringRectTransform resumeButton;
    public HoveringRectTransform settingButton;
    public HoveringRectTransform titleButton;
    public HoveringRectTransform exitButton;

    public override void ConfirmAction()
    {
        resumeButton.GetComponent<CustomButton>().OnClick();
    }

    public override HoveringRectTransform FindIndexButton(int index)
    {

        switch (index)
        {
            case 0:
                return resumeButton;
            case 1:
                return settingButton;
            case 2:
                return titleButton;
            case 3:
                return exitButton;
            default: return null;
        }
    }

    public void ResumeGame()
    {
        GameManager.UIManager.endMenu();
    }

    public void Setting()
    {
        GameManager.SettingButton();
    }

    public void GotoTitle()
    {
        GameManager.Scene.GotoTitle();
    }

    public void ExitGame()
    {
        GameManager.GameEnd();
    }
}
