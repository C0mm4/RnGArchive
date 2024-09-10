using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    public List<CustomButton> buttons;
    public bool isLoadDataExists;
    public int buttonIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (System.IO.File.Exists("SaveData.dat"))
        {
            isLoadDataExists = true;
        }
        else
        {
            isLoadDataExists= false;
        }
        buttonIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        if(buttonIndex == -1)
        {
            if (Input.anyKeyDown)
            {
                buttonIndex = 0;
            }
        }
        else
        {

            if (Input.GetKeyDown(GameManager.Input._keySettings.upKey))
            {
                ExecuteEvents.Execute(buttons[buttonIndex].gameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                if (buttonIndex > 2)
                {
                    buttonIndex = 0;
                }
                else
                {
                    buttonIndex--;
                    if (buttonIndex < 0)
                    {
                        buttonIndex = 2;
                    }
                    else if (buttonIndex == 1)
                    {
                        if (!isLoadDataExists)
                        {
                            buttonIndex = 0;
                        }
                    }
                }
                ExecuteEvents.Execute(buttons[buttonIndex].gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
            }

            if (Input.GetKeyDown(GameManager.Input._keySettings.downKey))
            {
                ExecuteEvents.Execute(buttons[buttonIndex].gameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                if (buttonIndex > 2)
                {
                    buttonIndex = 2;
                }
                else
                {
                    buttonIndex++;
                    if (buttonIndex > 2)
                    {
                        buttonIndex = 0;
                    }
                    else if (buttonIndex == 1)
                    {
                        if (!isLoadDataExists)
                        {
                            buttonIndex = 2;
                        }
                    }
                }
                ExecuteEvents.Execute(buttons[buttonIndex].gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
            }

            if (Input.GetKeyDown(GameManager.Input._keySettings.leftKey))
            {
                ExecuteEvents.Execute(buttons[buttonIndex].gameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                if (buttonIndex == 4)
                {
                    buttonIndex = 0;
                }
                else
                {
                    if (buttonIndex < 3)
                    {
                        buttonIndex = 3;
                    }
                }
                ExecuteEvents.Execute(buttons[buttonIndex].gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
            }

            if (Input.GetKeyDown(GameManager.Input._keySettings.rightKey))
            {
                ExecuteEvents.Execute(buttons[buttonIndex].gameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                if (buttonIndex == 3)
                {
                    buttonIndex = 0;
                }
                else
                {
                    if (buttonIndex < 3)
                    {
                        buttonIndex = 4;
                    }
                }
                ExecuteEvents.Execute(buttons[buttonIndex].gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
            }

            if (Input.GetKeyDown(GameManager.Input._keySettings.Interaction))
            {
                if (buttonIndex != -1)
                {
                    buttons[buttonIndex].GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }
}
