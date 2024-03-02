using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractionTrigger
{
    public Vector2 InDir;

    public string id;
    public string connectRoomId;
    public string connectDoorId;

    public override async void Interaction()
    {
        base.Interaction();
        await GameManager.player.GetComponent<PlayerController>().OutDoor(this);
        await GameManager.Scene.MoveMap(connectRoomId, connectDoorId);
    }
}
