using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public  class Supporter : Contents
{
    public SupporterData data;
    public bool isCool = false;


    public void Action() 
    {
        if (!isCool)
        {
            Vector3 playerPos = GameManager.Player.transform.position;
            GameObject go = GameManager.InstantiateAsync("Utaha", playerPos);
            isCool = true;
            data.leftCoolTime = data.coolTime;
        }
    }

}
