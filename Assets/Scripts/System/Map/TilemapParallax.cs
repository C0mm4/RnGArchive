using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapParallax : Obj
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    public override void OnCreate()
    {
        base.OnCreate(); 
        startpos = transform.position.x;
        length = GetComponent<TilemapRenderer>().bounds.size.x;
        cam = GameObject.Find("Main Camera");
    }

    public override void Step()
    {
        base.Step();
        if(cam == null)
        {
            cam = GameObject.Find("Main Camera");
        }
        else
        {
            float temp = (cam.transform.position.x * (1 - parallaxEffect));
            float dist = (cam.transform.position.x * parallaxEffect);

            transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;

        }
    }
}
