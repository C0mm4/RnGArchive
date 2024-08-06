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

    public int maxAmmo;
    public int currentAmmo;
    public float reloadT;

    public List<string> skins;
    public int _currentSkin;
    public int currentSkin { get { return _currentSkin; }  set { _currentSkin = value; } }

    public List<string> haloSkins;
    public int currentHalo;

    public string ProfileImg;
}
