using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class NPC : InteractionTrigger
{
    // Say Script
    public string script;

    // Distance to say
    public float sayDistance;
    
    public bool isSaying;

    public int sayingIndex;

    [SerializeField]
    public float sayingDelay;
    public float nextSayDelay;
    public GameObject letterBox;

    public override void OnCreate()
    {
        base.OnCreate();
        sayDistance = 5f;
        sayingIndex = 0;
        isSaying = false;
    }

    public override async void Step()
    {
        base.Step();
        if (player != null)
        {
            if(GameManager.GetUIState() == UIManager.UIState.InPlay)
            {
                if (PlayerDistance() <= sayDistance)
                {
                    if (!isSaying && script.Length > 0)
                    {
                        isSaying = true;
                        await Say();
                    }

                }
                else
                {
                    if (isSaying)
                    {
                        sayingIndex = script.Length;
                    }
                }
            }
        }
    }

    public void SetScript(string txt)
    {
        script = txt;
    }

    public async Task Say()
    {

        if (letterBox != null)
        {
            GameManager.Destroy(letterBox);
        }

        isSaying = true;
        sayingIndex = 0;

        var awaitObj = GameManager.InstantiateAsync("LetterBox");
        letterBox = awaitObj;
        letterBox.GetComponent<LetterBox>().npc = this;
        letterBox.GetComponent<LetterBox>().SetPosition();
        letterBox.transform.SetParent(GameManager.UIManager.canvas.transform, false);


        while (sayingIndex <= script.Length - 1)
        {
            if (!isSaying)
            {
                break;
            }
            sayingIndex++;
            if(sayingIndex < script.Length)
            {
                if (script[sayingIndex].Equals(" "))
                {
                    sayingIndex++;
                }
            }
            var cuttext = script[..sayingIndex];
            
            letterBox.GetComponent<LetterBox>().SetText(cuttext);
            await Task.Delay(TimeSpan.FromSeconds(sayingDelay));
        }

        if(sayingIndex == letterBox.GetComponentInChildren<TMP_Text>().text.Length)
        {
            await Task.Delay(TimeSpan.FromSeconds(1f));
        }

        GameManager.Destroy(letterBox);

        await Task.Delay(TimeSpan.FromSeconds(nextSayDelay));
        

        isSaying = false;
    }

}
