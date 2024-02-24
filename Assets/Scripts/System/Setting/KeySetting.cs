using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeySettings", menuName = "ScriptableObjects/KeySettings", order = 1)]
public class KeySettings : ScriptableObject
{
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;

    public KeyCode Shot = KeyCode.A;
    public KeyCode Jump = KeyCode.S;
    public KeyCode Call = KeyCode.D;
    public KeyCode Interaction = KeyCode.F;
}