using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ArisCleanUp : Skill
{
    float startT;
    bool isCharge = false;
    float chargeT;
    bool isRunning = false;

    public override void PassiveEffect()
    {

    }

    public override async void Step()
    {
        if (!isRunning)
        {
            isRunning = true;
            player.isAction = true;
            player.canMove = false;
            GameObject Fire = GameManager.InstantiateAsync("ArisCleanUp", player.transform.position);
            if(player.sawDir.x < 0)
            {
                Fire.GetComponent<SpriteRenderer>().flipX = true;
            }
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
            Fire.GetComponent<Bullet>().CreateHandler(10, player.sawDir, AtkType.Piercing);
            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            player.charactor.EndState();
            player.canMove = true;
            player.isAction = false;
            player.workingSkill = null;
            isRunning = false;
        }

    }

    public override void PassiveStep()
    {
        if (isCharge)
        {
            chargeT = Time.time - startT;
        }
        base.PassiveStep();
        if (Input.GetKeyDown(GameManager.Input._keySettings.Shot) && !player.isSit)
        {
            if(currentAmmo > 0)
            {
                startT = Time.time;
                isCharge = true;
            }
            Debug.Log(currentAmmo);
        }

        if(Input.GetKeyUp(GameManager.Input._keySettings.Shot))
        {
            isCharge = false;
            if (chargeT > 5f)
            {
                Debug.Log("Full Charge Shot");
                Execute(player.sawDir);
            }
            else if (chargeT > 3f)
            {
                Debug.Log("Charge Shot");
                Execute(player.sawDir);
            }
            chargeT = 0;
            player.isAction = false;
        }
    }

    public override void Execute(Vector2 dir)
    {
        base.Execute(dir);
    }
}
