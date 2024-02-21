using System.Collections.Generic;

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
}