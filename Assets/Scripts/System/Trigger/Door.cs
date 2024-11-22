using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractionTrigger
{
    public Vector2 InDir;

    public Sprite openSpr;
    public Sprite closeSpr;

    SpriteRenderer renderer;

    public string id;
    public string connectRoomId;
    public string connectDoorId;

    public bool isActivate;

    public override void OnCreate()
    {
        base.OnCreate();
        text = "이동한다";
        if (isActivate)
        {
            detectDistance = 1f;
        }

        renderer = GetComponent<SpriteRenderer>();
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

    public void DoorActivate()
    {
        isActivate = true;
        renderer.sprite = openSpr;
    }

    public void DoorDeActivate()
    {
        renderer.sprite = closeSpr;
    }

}
