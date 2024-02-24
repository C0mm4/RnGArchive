using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharactorData 
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
