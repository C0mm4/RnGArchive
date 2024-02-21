using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SerializeStatus 
{
    public string Name;
    public int id;
    public float moveAccelSpeed;
    public float breakAccel;
    public float maxSpeed;
    public float activeMaxSpeed;

    public float attackSpeed;

    public string prefabPath;

    public List<string> skins;
}
