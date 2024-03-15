using System;

[Serializable]
public class SerializeStatus 
{
    public int maxHP;
    public int currentHP;


}


public enum UIState
{
    Loading, InPlay, Title, CutScene, Menu, Pause,
};