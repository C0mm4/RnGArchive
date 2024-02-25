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
        script = "������ �帮���� ������! �� �����̶� ���� ���� �������κ��� ������ ����� �״�� �̾�޾� 2�� â���Ͽ� ���ο� ������ ������ ������ �����ϴ� ���忡���� ������ ������ ĳ����, ������ ����ϹǷ� �̹� ����� ������� �̿��ϱ⿡ ��ȹ�ϱ� ���ٶ�� ������ ������, ȫ���ϴ� ���������� �ش� ���� ����Ʈ�� ���̸� Ÿ������ ���۵Ǳ� ������ ���� ���۵Ǵ� ������ ġ�� ���� ȫ�� ������ ���� �� �ִٴ� ������ ������ ����!";
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
