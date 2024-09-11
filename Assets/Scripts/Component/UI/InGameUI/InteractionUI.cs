using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InteractionUI : Obj
{
    PlayerController player;
    public string interactionUI;
    public List<GameObject> interactionUIs = new();
    
    public override void Step()
    {
        base.Step();
        if(player == null)
        {
            GameObject playerObj = GameManager.player;
            if(playerObj != null)
            {
                player = playerObj.GetComponent<PlayerController>();
            }
        }
        else
        {
            if(player.triggers.Count > 0)
            {
                GameManager.Destroy(interactionUIs.ToArray());
                interactionUIs.Clear();
                int cnt = player.triggers.Count;
                for(int i = 0; i < cnt; i++)
                {
                    GameObject go = GameManager.InstantiateAsync(interactionUI);
                    interactionUIs.Add(go);
                    go.transform.SetParent(transform, false);
                    go.transform.localPosition = new Vector3(0, i * -60, 0);
                    go.GetComponentInChildren<TMP_Text>().text = player.triggers[i].text;
                }
            }
            else
            {
                Destroy();
            }
        }

        if(GameManager.GetUIState() != UIState.InPlay)
        {
            Destroy();
        }
        

        if(player == null)
        {
            Destroy();
        }
    }
}
