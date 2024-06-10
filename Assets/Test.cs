using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool isAction = false;
    public int sayingIndex = 0;
    public float sayT = 0f;
    // Start is called before the first frame update
    void Start()
    {/*
        string test = "가나다라 #Delay.10 아자차카타파하";
        Debug.Log(test.Length);
        for(int i = 0; i < test.Length; i++)
        {
            if (test[i].Equals('#'))
            {
                Debug.Log(test[..i]);
                string action;
                int actionStartIndex = i;
                while (test[i] != ' ')
                {
                    i++;
                }
                action = test[actionStartIndex..i];

                test = test.Remove(actionStartIndex, i - actionStartIndex);

                i = actionStartIndex - 1;


                Debug.Log(action);
            }
        }

        Debug.Log(test);*/
    }

    public void Update()
    {
        if (!isAction)
        {
            isAction = true;
            A("가나다라마 #Delay.10 아자차카타파하");
        }
    }

    public async void A(string script)
    {
        await Say(script);
    }

    public async Task Say(string script)
    {
        Debug.Log(script);
        while(sayingIndex <= script.Length - 1)
        {
            sayT += Time.deltaTime;
            if(sayT >= 0.2f)
            {
                sayT = 0f;

                if (script[sayingIndex].Equals('#'))
                {
                    int actionStartIndex = sayingIndex;
                    string action;
                    while (!script[sayingIndex].Equals(' '))
                    {
                        sayingIndex++;
                    }
                    action = script[actionStartIndex..sayingIndex++];
                    await Action(action);
                    script = script.Remove(actionStartIndex, sayingIndex - actionStartIndex);

                    sayingIndex = actionStartIndex;
                }
                else
                {
                    sayingIndex++;
                    var cuttext = script[..sayingIndex];
                    Debug.Log(cuttext);
                }


                
            }
            await Task.Yield();
        }
    }

    public async Task Action(string action)
    {
        switch (action.Split('.')[0])
        {
            case "#Delay":
                Debug.Log(action.Split('.')[1]);
                break;
            case "Camera":
                break;
            case "Spawn":
                break;

        }
    }
}
