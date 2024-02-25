using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : Obj
{

    public PlayerController player;
    public float detectDistance;


    public override void OnCreate()
    {
    }

    public override void Step()
    {
        base.Step();
        if(player == null)
        {
            FindPlayer();
        }
        else if(PlayerDistance() <= detectDistance)
        {
            player.AddInterractionTrigger(this);
        }
        else
        {
            player.RemoveInteractionTrigger(this);
        }
    }

    public float PlayerDistance()
    {
        float distance = Vector2.Distance(transform.position, GameManager.player.transform.position);

        return distance;
    }

    public void FindPlayer() 
    {
        if(GameManager.player != null)
        {
            player = GameManager.player.GetComponent<PlayerController>();
        }
    }

    public virtual void Interaction()
    {

    }
}
