using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
    public List<Button> buttons;
    public Color pressedColor;
    private Color originalColor = Color.white;
    private Button activateButton;

    public MapCreateController controller;

    void Start()
    {
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
            button.onClick.AddListener(() => SetCursorMode(buttons.IndexOf(button)));
        }

        buttons[0].onClick.Invoke();
    }

    private void OnButtonClick(Button button)
    {
        if (activateButton == button) return;

        ResetAllButtons();
        button.GetComponent<Image>().color = pressedColor;
        activateButton = button;
    }

    private void ResetAllButtons()
    {
        foreach( Button button in buttons)
        {
            button.GetComponent<Image> ().color = originalColor;
        }
    }

    public void SetButton(int i)
    {
        buttons[i].onClick.Invoke();
    }

    public void SetCursorMode(int index)
    {
        switch (index)
        {
            case 0:
                controller.inputMode = MapCreateController.InputMode.draw;
                break;
            case 1:
                controller.inputMode = MapCreateController.InputMode.erase;
                break;
            case 2:
                controller.inputMode = MapCreateController.InputMode.selectObj;
                break;
            case 3:
                controller.inputMode = MapCreateController.InputMode.selectTrigger;
                break;
            case 4:
                controller.inputMode = MapCreateController.InputMode.drawCutSceneTrigger;
                break;
            case 5:
                controller.inputMode = MapCreateController.InputMode.drawSpawnTrigger;
                break;
            case 6:
                controller.inputMode = MapCreateController.InputMode.drawSpawnPoint;
                break;
        }
    }
}
