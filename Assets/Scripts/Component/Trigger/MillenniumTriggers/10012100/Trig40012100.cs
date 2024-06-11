using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class Trig40012100 : CutSceneTrigger
{
    public Transform utahaSpawn;
    public Door Door;


    public override async Task Action()
    {
        
        PlayerController player = GameManager.Player.GetComponent<PlayerController>();
        Door.isActivate = false;
        GameManager.Scene.CreateBlackOutObj();
        await base.Action();
    }

    public override bool AdditionalCondition()
    {
        return true;
    }
}
