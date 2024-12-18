using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "MobData", menuName = "ScriptableObjects/MobData", order = 1)]
public class MobData : ScriptableObject
{
    public int maxHP;
    public float maxSpeed;
    public float speedAccel;
    public float[] attackDelay;
    public bool[] attackIsCool;
    public float detectDistance;
    public int jumpForce;

    public AtkType atkType;
    public DefType defType;

    public GameObject[] atkPref;

    public string AIModel;
}
