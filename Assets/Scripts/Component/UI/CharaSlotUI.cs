using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CharaSlotUI : Obj
{
    public int index;
    public Image charaIcon;
    public Image HPBar;

    public int currentCharaId;
    [SerializeField]
    Charactor target;


    public float movingT = 0;
    public float enableXePos = 180, disableXPos = -310;

    RectTransform rect;

    public List<Image> emptyCostBar;
    public List<Image> fillInCostBar;
    private bool isCostSet = false;

    public GameObject ChangeUI;

    // 1 = enableAnimation, 2 = enable, 3 = disableAnimation, 0 = disable, 4 = hitAnimation
    public int state;

    public override void OnCreate()
    {
        base.OnCreate();
        rect = GetComponent<RectTransform>();
        DisableChangeSlot();
    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        try
        {
            
            Charactor targetCharactor = GameManager.Progress.charaDatas[GameManager.Progress.currentParty[index].charaData.id].charactor;
            if(targetCharactor != null)
            {
                gameObject.SetActive(true);
                HPBar.fillAmount = (float)targetCharactor.charaData.currentHP / (float)targetCharactor.charaData.maxHP;
                

                if (!isCostSet && target != null)
                {
                    int maxCost = ((int)target.charaData.maxCost);
                    int i;
                    for(i = 0; i < maxCost; i++) 
                    {
                        emptyCostBar[i].color = Color.white;
                    }
                    for(; i < 10; i++)
                    {
                        emptyCostBar[i].color = new Color(1, 1, 1, 0);
                    }
                    isCostSet = true;
                }

                float targetCost = target.charaData.currentCost;
                int manaIndex = 0;
                while(targetCost > 0)
                {
                    if(targetCost >= 1f)
                    {
                        fillInCostBar[manaIndex].fillAmount = 1f;
                        manaIndex++;
                        targetCost -= 1f;
                    }
                    else
                    {
                        fillInCostBar[manaIndex].fillAmount = targetCost;
                        targetCost = 0f;
                        manaIndex++;
                    }
                }
                for(; manaIndex < 10; manaIndex++)
                {
                    fillInCostBar[manaIndex ].fillAmount = 0f;
                }

            }
            else
            {
                
                gameObject.SetActive(false);
            }

        }
        catch
        {

            gameObject.SetActive(false);
        }
    }

    public async void Enable(float t = 0, float time = 1f)
    {
        state = 1;
        gameObject.SetActive(true);
        isCostSet = false;
        target = GameManager.Progress.charaDatas[GameManager.Progress.currentParty[index].charaData.id].charactor;
        currentCharaId = target.charaData.id;
        movingT = t;
        while (movingT < time)
        {
            if(GameManager.GetUIState() != UIState.InPlay)
            {
                return;
            }
            float targetX = Mathf.Lerp(rect.localPosition.x, enableXePos - Screen.width / 2, movingT);
            rect.localPosition = new Vector3(targetX, rect.localPosition.y);
            movingT += Time.deltaTime * 2;
            await Task.Yield();
        }

        rect.localPosition = new Vector3(enableXePos - Screen.width / 2, rect.localPosition.y);
        movingT = 0f;
        state = 2;
    }

    public async void Disable(float t = 0, float time = 1f)
    {
        state = 3;
        bool playerChange = false;
        if(GameManager.uiState == UIState.InPlay)
        {
            playerChange = true;
        }
        movingT = t;
        while (movingT < time)
        {
            if(GameManager.uiState == UIState.InPlay && !playerChange)
            {
                return;
            }
            float targetX = Mathf.Lerp(rect.localPosition.x, disableXPos - Screen.width / 2, movingT);
            rect.localPosition = new Vector3(targetX, rect.localPosition.y);
            movingT += Time.deltaTime * 2;
            await Task.Yield();
        }

        rect.localPosition = new Vector3(disableXPos - Screen.width / 2, rect.localPosition.y);
        movingT = 0f;
        target = null;
        gameObject.SetActive(false);
        state = 0;
    }

    public void DisableAfterAnimation()
    {
        gameObject.SetActive(false);
    }

    [System.Obsolete]
    public async void HitAnimation()
    {
        state = 4;
        Vector2 originPos = rect.localPosition;
        float t = 0f;
        while(state == 4)
        {
            if(t >= 0.3f)
            {
                break;
            }
            rect.localPosition += new Vector3(Random.RandomRange(-10, 10), Random.RandomRange(-10, 10));
            t += Time.deltaTime;
            await Task.Yield();
        }
        if(state == 4)
        {
            state = 2;
            rect.localPosition = originPos;
        }
        
    }

    public void EnableChangeSlot()
    {
        ChangeUI.SetActive(true);
    }

    public void DisableChangeSlot() 
    {
        ChangeUI.SetActive(false);
    }

}
