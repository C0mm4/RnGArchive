using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController
{

    public Charactor currentCharactor;
    public List<Charactor> party = new();
    public int currentIndex;


    // Update is called once per frame
    public void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (party.Count > 1)
            {
                currentIndex++;
                currentIndex %= party.Count;

                GameManager.NextCharactor();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.UIManager.MapToggle();
        }
    }

    public void SetInitializeParty()
    {
        party.Add(GameManager.CharaCon.charactors[10001001]);
        party.Add(GameManager.CharaCon.charactors[10001002]);
    }
}
