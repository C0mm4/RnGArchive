using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArisCleanUp : Skill
{
    float startT;
    bool isCharge = false;
    float chargeT;

    public override void PassiveEffect()
    {

    }

    public override void Step()
    {

    }

    public override void PassiveStep()
    {
        if (isCharge)
        {
            chargeT = Time.time - startT;
            Debug.Log(chargeT);
//            player.isAction = true;
        }
        base.PassiveStep();
        if (Input.GetKeyDown(GameManager.Input._keySettings.Shot) && !player.isSit)
        {
            startT = Time.time;
            isCharge = true;
        }

        if(Input.GetKeyUp(GameManager.Input._keySettings.Shot))
        {
            isCharge = false;
            if (chargeT > 5f)
            {
                Debug.Log("Full Charge Shot");
            }
            else if (chargeT > 3f)
            {
                Debug.Log("Charge Shot");
            }
            chargeT = 0;
            player.isAction = false;
        }
    }
}
