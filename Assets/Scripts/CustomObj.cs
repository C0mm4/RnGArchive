using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomObj : Obj
{
    public Vector3 dir = new Vector3();


    public override void BeforeStep()
    {
        base.BeforeStep();
        dir = Vector3.zero;
    }

    public override void Step()
    {
        base.Step();
        transform.position += dir;
    }

    public override void AfterStep()
    {
        base.AfterStep();
    }

    public override void KeyInput()
    {

    }


    public override void Alarm0()
    {
        Debug.Log("Alarm0 Activate");
    }

    public override void Alarm1()
    {
        Debug.Log("Alarm1 Activate");
        Destroy(gameObject);
    }

    public override void Alarm2()
    {
        throw new System.NotImplementedException();
    }

    public override void Alarm3()
    {
        throw new System.NotImplementedException();
    }

    public override void Alarm4()
    {
        throw new System.NotImplementedException();
    }

    public override void Alarm5()
    {
        throw new System.NotImplementedException();
    }

    public override void Alarm6()
    {
        throw new System.NotImplementedException();
    }

    public override void Alarm7()
    {
        throw new System.NotImplementedException();
    }

    public override void Alarm8()
    {
        throw new System.NotImplementedException();
    }

    public override void Alarm9()
    {
        throw new System.NotImplementedException();
    }


}
