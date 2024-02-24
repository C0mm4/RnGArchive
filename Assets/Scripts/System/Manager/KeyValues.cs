using System.Collections.Generic;
using System.Text;

public enum KeyValues
{
    None = 0, O = 128,
    // Base Keys
    Up = 1, Down = 2, Left = 4, Right = 8, Shot = 16, Jump = 32, Call = 64,
    // Arrows Combinate Keys
    UpLeft = 5, UpRight = 9, DownLeft = 6, DownRight = 10,
    // Arrow, Action Combinate Keys
    UpShot = 17, UpJump = 33, UpCall = 65,
    DownShot = 18, DownJump = 34, DownCall = 66,
    LeftShot = 20, LeftJump = 36, LeftCall = 68,
    RightShot = 24, RightJump = 40, RightCall = 72,

    // Arrows, Action Combinate Keys
    UpLeftShot = 21, UpLeftJump = 37, UpLeftCall = 69,
    UpRightShot = 25, UpRightJump = 41, UpRightCall = 73,
    DownLeftShot = 22, DownLeftJump = 38, DownLeftCall = 70,
    DownRightShot = 26, DownRightJump = 42, DownRightCall = 74,

};

public enum AtkType
{
    Normal, Explosive, Piercing, Mystic, Sonic,
}

public enum DefType
{
    Normal, Light, Heavy, Special, Elastic,
}

public static class Func
{
    public static KeyValues Reverse(KeyValues kv)
    {
        KeyValues revKv = kv;
         if ((kv & KeyValues.Left) == KeyValues.Left)
         {
            revKv = kv - KeyValues.Left + KeyValues.Right;
         }
         else if((kv & KeyValues.Right) == KeyValues.Right)
         {
             revKv = kv - KeyValues.Right + KeyValues.Left;
         }
 
         return revKv;
    }
    public static List<KeyValues> Reverse(List<KeyValues> kvs)
    {
        List<KeyValues> revKvs = new();
        foreach(KeyValues kv in kvs)
        {
            revKvs.Add(Reverse(kv));
        }
        return revKvs;
    }

    public static float GetDmgMultiplier(AtkType atk, DefType def)
    {
        switch (atk)
        {
            case AtkType.Normal:
                return 1;
            case AtkType.Explosive:
                switch (def)
                {
                    case DefType.Normal :
                        return 1;
                    case DefType.Light:
                        return 2;
                    case DefType.Heavy:
                        return 1f;
                    case DefType.Special:
                        return 0.4f;
                    case DefType.Elastic:
                        return 0.4f;
                }
                break;
            case AtkType.Piercing:
                switch (def)
                {
                    case DefType.Normal:
                        return 1f;
                    case DefType.Light:
                        return 0.4f;
                    case DefType.Heavy:
                        return 2f;
                    case DefType.Special:
                        return 1f;
                    case DefType.Elastic:
                        return 1f;
                }
                break;
            case AtkType.Mystic:
                switch (def)
                {
                    case DefType.Normal:
                        return 1;
                    case DefType.Light:
                        return 1;
                    case DefType.Heavy:
                        return 0.4f;
                    case DefType.Special:
                        return 2f;
                    case DefType.Elastic:
                        return 1f;
                }
                break;
            case AtkType.Sonic:
                switch (def)
                {
                    case DefType.Normal:
                        return 1;
                    case DefType.Light:
                        return 1;
                    case DefType.Heavy:
                        return 0.4f;
                    case DefType.Special:
                        return 1.5f;
                    case DefType.Elastic:
                        return 2f;
                }
                break;
        }
        return 1;
    }

    public static List<KeyValues> arrows = new() { KeyValues.Up, KeyValues.Left, KeyValues.Down, KeyValues.Right};
    public static List<KeyValues> directions = new() { KeyValues.Up, KeyValues.UpLeft, KeyValues.Left, KeyValues.DownLeft, KeyValues.Down, KeyValues.DownRight, KeyValues.Right, KeyValues.UpRight };
    public static List<KeyValues> actions = new() { KeyValues.Shot, KeyValues.Jump, KeyValues.Call };
}