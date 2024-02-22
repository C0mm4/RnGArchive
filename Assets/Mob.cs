using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mob : KinematicObject
{
    // Mob Attack Prefab
    [SerializeField]
    public GameObject[] attackPrefab;
    public bool[] isHitbox;
    public List<float> lastAttackT = new List<float>();
    public MobData data;

    public AIModel AI;

    public override void OnCreate()
    {
        base.OnCreate();
    }

    public void CreateHandler()
    {
        AIGen();
    }

    public void AIGen()
    {

    }

    public override void Step()
    {
        base.Step();
        AI.Step();
    }

    protected override void ComputeVelocity()
    {
        targetVelocity.x = moveAccel.x;
    }

    public override void CheckCollision(RaycastHit2D rh, bool yMovement)
    {
        base.CheckCollision(rh, yMovement);

    }



    // Attack Object Generate
    public void AttackObjGen(int index)
    {

    }


}
