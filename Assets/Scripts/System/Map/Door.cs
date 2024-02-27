using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractionTrigger
{
    public Vector2 InDir;

    public int id;
    public int connectRoomId;
    public int connectDoorId;

    public override async void Interaction()
    {
        base.Interaction();
        await GameManager.player.GetComponent<PlayerController>().OutDoor(this);

    }
}
