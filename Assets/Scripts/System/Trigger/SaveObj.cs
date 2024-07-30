using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObj : InteractionTrigger
{
    float rotateSpeed = 180f;
    float rotate = 0f;

    public override void OnCreate()
    {
        base.OnCreate();
        text = "저장한다";
    }
    public override void Interaction()
    {
        base.Interaction();
        GameManager.Save.SaveGameprogress(transform);
    }

    public override void Step()
    {
        base.Step();
        rotate += Time.deltaTime * rotateSpeed;
        transform.rotation = Quaternion.Euler(0f, rotate, 1f);
    }
}
