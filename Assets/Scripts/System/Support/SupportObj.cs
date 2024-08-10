using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportObj : PlayerTest
{
    protected bool isAttack = false;

    public int HP;
    public AtkType atkType;
    public DefType defType;

    public float spawnT;

    SupporterData data;

    public override void OnCreate()
    {
        base.OnCreate();
        
    }

    public void CreateHandler(SupporterData data)
    {
        this.data = data;
        SetType(data.atkType, data.defType);
        SetAlarm(1, data.durateT);
    }

    public virtual void Attack()
    {

    }

    public override void Alarm0()
    {
        base.Alarm0();
        isAttack = false;
    }

    public override void Alarm1()
    {
        base.Alarm1();
        Destroy();
    }


    public void SetType(string atkT, string defT)
    {
        atkT = atkT.ToLower();
        defT = defT.ToLower();
        switch (atkT)
        {
            case "explosive":
                atkType = AtkType.Explosive;
                break;
            case "piercing":
                atkType = AtkType.Piercing;
                break;
            case "mystic":
                atkType = AtkType.Mystic;
                break;
            case "sonic":
                atkType = AtkType.Sonic;
                break;
            case "normal":
                atkType = AtkType.Normal;
                break;
        }
        switch (defT)
        {
            case "normal":
                defType = DefType.Normal;
                break;
            case "light":
                defType = DefType.Light;
                break;
            case "heavy":
                defType = DefType.Heavy;
                break;
            case "special":
                defType = DefType.Special;
                break;
            case "elastic":
                defType = DefType.Elastic;
                break;
        }
    }
}
