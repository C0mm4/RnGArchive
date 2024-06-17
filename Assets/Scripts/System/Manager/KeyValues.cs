using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

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

    public static void SetRectTransform(GameObject go, Vector3 pos = default)
    {
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.transform.SetParent(GameManager.UIManager.canvas.transform, false);
        rt.transform.localScale = Vector3.one;
        rt.transform.localPosition = pos;
    }

    public static List<KeyValues> arrows = new() { KeyValues.Up, KeyValues.Left, KeyValues.Down, KeyValues.Right};
    public static List<KeyValues> directions = new() { KeyValues.Up, KeyValues.UpLeft, KeyValues.Left, KeyValues.DownLeft, KeyValues.Down, KeyValues.DownRight, KeyValues.Right, KeyValues.UpRight };
    public static List<KeyValues> actions = new() { KeyValues.Shot, KeyValues.Jump, KeyValues.Call };

    public static string ChangeStringToValue(string txt)
    {
        string pattern = @"\{(?<varName>.+?)\}";

        // �ؽ�Ʈ ���ڿ�
        string ret = txt;

        // ���Խ��� ����Ͽ� ���� ����
        MatchCollection matches = Regex.Matches(ret, pattern);

        // �� ������ ���� ���� �����ͼ� �ؽ�Ʈ�� ��ü
        foreach (Match match in matches)
        {
            string varName = match.Groups["varName"].Value;

            // �������� ������� �ش� ������ ���� ������
            string value = GetValueFromVariableName(varName);

            // �ؽ�Ʈ���� ������ �ش� ������ ��ü
            ret = ret.Replace(match.Value, value);
        }

        return ret;
    }

    // �������� ������� �ش� ������ ���� �������� �޼���
    static string GetValueFromVariableName(string varName)
    {
        // ���⼭�� �ܼ� ������ ������ �� ��ȯ
        // �����δ� Reflection ���� ����Ͽ� �������� ���� �� ������ �� ����
        if (varName == "GameManager.Input._keySettings.Call")
        {
            return GameManager.Input._keySettings.Call.ToString();
        }
        else
        {
            // �ش� ������ ���� ���� ���� ��� ó��
            return $"Unknown variable: {varName}";
        }
    }

    // Swap List Objects
    public static void Swap<T>(List<T> list, int index1, int index2)
    {
        if (index1 < 0 || index1 >= list.Count || index2 < 0 || index2 >= list.Count)
        {
            throw new ArgumentOutOfRangeException("�ε����� ����Ʈ�� ������ ������ϴ�.");
        }

        T temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }


    public static async Task Action(string action)
    {
        action = action.TrimStart('#');
        action = action.Trim(' ');
        string[] actions = action.Split('.');

        string targetNPCID;

        for(int i = 0; i < actions.Length; i++)
        {
            switch (actions[i])
            {
                case "Delay":
                    float delayT = float.Parse(actions[++i]);
                    Debug.Log(delayT);
                    await Task.Delay(TimeSpan.FromMilliseconds(delayT));
                    break;
                case "Camera":
                    targetNPCID = actions[++i];
                    if (targetNPCID.Equals("Player"))
                    {
                        GameManager.CameraManager.player = GameManager.Player.transform;
                    }
                    else
                    {
                        NPC npc = FindNPC(targetNPCID, actions[++i]);
                        GameManager.CameraManager.player = npc.transform;
                    }
                    break;
                case "Spawn":
                    targetNPCID = actions[++i];
                    if (targetNPCID[0].Equals('2'))
                    {
                        FindNPC(targetNPCID, actions[++i]);
                    }
                    break;
                case "Animation":
                    targetNPCID = actions[++i];
                    if (targetNPCID.Equals("Player"))
                    {
                        GameManager.player.GetComponent<PlayerController>().AnimationPlayBody(actions[++i]);
                    }
                    else
                    {
                        NPC npc = FindNPC(targetNPCID, actions[++i]);
                        npc.AnimationPlay(npc.animator, actions[++i]);
                    }
                    break;
            }
        }
    }

    public static NPC FindNPC(string id, string spawnP)
    {
        List<NPC> npcs = GameManager.Instance.currentMapObj.GetComponentsInChildren<NPC>().ToList();
        NPC ret = npcs.Find(item => item.npcId.Equals(id));

        if(ret == null)
        {
            List<SpawnP> transes = GameManager.Instance.currentMapObj.GetComponentsInChildren<SpawnP>().ToList();
            Transform trans = transes.Find(item => item.id.Equals(spawnP)).transform;
            ret = GameManager.MobSpawner.NPCSpawn(id, trans.position);
        }

        return ret;
    }

    public static string PlayerIDToNPCID(string id)
    {
        switch (id)
        {
            case "10001002":
                return "20001003";
            default:
                return null;
        }
    }
}