using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CharactorData 
{
    public string Name;
    public int id;
    public float maxSpeed;
    public float activeMaxSpeed;
    public float jumpForce;

    public float attackSpeed;

    public string prefabPath;

    public int maxHP;
    public int currentHP;

    public List<string> skins;
    public int currentSkin;
}
