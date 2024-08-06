using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeySetting", menuName = "ScriptableObjects/KeySettings", order = 1)]
public class KeySetting : ScriptableObject
{
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    public KeyCode Shot;
    public KeyCode Jump;
    public KeyCode Call;
    public KeyCode Interaction;

    public KeyCode Skill1;
    public KeyCode Skill2;
    public KeyCode Skill3;

    public KeyCode Reload;
}