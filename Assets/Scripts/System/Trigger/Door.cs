using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractionTrigger
{
    public Vector2 InDir;

    public string id;
    public string connectRoomId;
    public string connectDoorId;

    public bool isActivate;

    public override void OnCreate()
    {
        base.OnCreate();
        text = "�̵��Ѵ�";
        if (isActivate)
        {
            detectDistance = 1f;
        }
    }

    public override void Step()
    {
        base.Step();
        if (isActivate)
        {
            detectDistance = 1f;
        }
        else
        {
            detectDistance = 0f;
        }
    }

    public override async void Interaction()
    {
        if (isActivate)
        {
            base.Interaction();
            await GameManager.Scene.MoveMap(connectRoomId, connectDoorId);
        }
    }
}
