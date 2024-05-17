using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Particle : Obj
{
    public Vector3 sPos;
    public Vector3 ePos;
    public float ableTime;
    float t;

    public Animator animator;

    public override void OnCreate()
    {
        base.OnCreate();

    }

    public void CreateHandler(Vector3 startPos, Vector3 endPos, float time = 1f) 
    {
        t = 0;
        sPos = startPos; ePos = endPos;
        transform.position = sPos;
        ableTime = time;

        animator = GetComponent<Animator>();
        AnimationPlay(animator, "Play", 1);
    }

    public override void BeforeStep()
    {
        base.BeforeStep();
        t += Time.deltaTime;
        transform.position = Vector3.Lerp(sPos, ePos, t / ableTime);
        if (t >= ableTime)
        {
            Destroy();
        }
    }


}
