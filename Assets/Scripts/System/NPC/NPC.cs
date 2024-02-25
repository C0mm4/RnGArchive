using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractionTrigger
{
    public string script;
    public float sayDistance;
    public bool isSaying = false;
    public bool isSayingDelay = false;
    public int sayingIndex = 0;

    [SerializeField]
    public float sayingDelay = 0.1f;
    public GameObject letterBox;

    public override void OnCreate()
    {
        base.OnCreate();
        sayDistance = 5f;
        script = "설명해 드리도록 하지요! 팬 게임이라 함은 원본 컨텐츠로부터 설정과 배경을 그대로 이어받아 2차 창작하여 새로운 게임을 만들어내는 것으로 개발하는 입장에서는 기존의 설정과 캐릭터, 배경등을 사용하므로 이미 구축된 세계관을 이용하기에 기획하기 쉽다라는 장점이 있으며, 홍보하는 과정에서도 해당 원본 컨텐트의 파이를 타겟으로 제작되기 때문에 새로 제작되는 컨텐츠 치고 쉬운 홍보 과정을 가질 수 있다는 장점을 가지고 있죠!";
        sayingIndex = 0;
        isSaying = false;
    }

    public override void Step()
    {
        base.Step();
        if (player != null)
        {
            if (PlayerDistance() <= sayDistance)
            {
                isSaying = true;
                if(letterBox == null)
                {
                    letterBox = GameManager.InstantiateAsync("LetterBox", transform.position);
                    letterBox.GetComponent<LetterBox>().npc = this;
                    letterBox.GetComponent<LetterBox>().SetPosition();
                    letterBox.transform.SetParent(GameManager.UIManager.canvas.transform, false);
                }
                if(sayingIndex < script.Length - 1)
                {
                    if (!isSayingDelay)
                    {
                        SetAlarm(0, sayingDelay);
                    }
                }
                else
                {
                    letterBox.GetComponent<LetterBox>().SetText(script);
                }

            }
            else if (PlayerDistance() > sayDistance)
            {
                if(isSaying)
                {
                    isSaying = false;
                    sayingIndex = 0;
                }
                else
                {
                    GameManager.Destroy(letterBox);
                }
            }
        }
    }

    public override void Alarm0()
    {
        if (isSaying)
        {
            sayingIndex++;
            if (script[sayingIndex].Equals(" "))
            {
                sayingIndex++;
            }
            var cuttext = script[..sayingIndex];
            isSayingDelay = true;
            letterBox.GetComponent<LetterBox>().SetText(cuttext);
            SetAlarm(1, sayingDelay);
        }
    }

    public override void Alarm1()
    {
        base.Alarm1();
        isSayingDelay = false;
    }

}
