using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ArisCleanUp : Skill
{
    bool isRunning = false;

    public override void PassiveEffect()
    {

    }

    public override async void Action()
    {
        if (!isRunning)
        {
            isRunning = true;
            player.isAction = true;
            player.canMove = false;
            GameObject Fire = GameManager.InstantiateAsync("ArisCleanUp", player.weapon.muzzle.transform.position, player.weapon.muzzle.transform.rotation);
            Fire.GetComponent<Obj>().AnimationPlay(Fire.GetComponent<Bullet>().animator, "Generate");

            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            Fire.GetComponent<Bullet>().CreateHandler(10, player.sawDir, AtkType.Piercing);
            player.weapon.AnimationPlay(player.weapon.animator, "Fire");
            Fire.GetComponent<Obj>().AnimationPlay(Fire.GetComponent<Bullet>().animator, "Shot");

            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            player.charactor.EndState();
            player.canMove = true;
            player.isAction = false;
            player.workingSkill = null;
            isRunning = false;
        }

    }
    public override void Execute(Vector2 dir)
    {
        base.Execute(dir);
    }

    public override bool ExecuteCondition()
    {
        return player.isGrounded && !isRunning;
    }

    public override void Step()
    {
    }
}
