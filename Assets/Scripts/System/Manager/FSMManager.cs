using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class FSMManager
{
    public Dictionary<string, List<string>> fsm;

    public void init()
    {
        fsm = new Dictionary<string, List<string>>();

        List<string> fsmList = new List<string>
        {
            // 0    1       2               3             4           5         6
            "Idle", "Move", "PrepareJump", "JumpFinish", "Falling", "Landing", "SpecialMove"
        };

        // Set Idle State Nodes
        List<string> nodes = new List<string> { fsmList[1], fsmList[2], fsmList[4] };
        fsm[fsmList[0]] = new List<string>(nodes);

        // Set Move State Nodes
        nodes = new List<string> { fsmList[0], fsmList[2], fsmList[6] };
        fsm[fsmList[1]] = new List<string>(nodes);

        // Set PrepareJump State Nodes
        nodes = new List<string> { fsmList[3], fsmList[6] };
        fsm[fsmList[2]] = new List<string>(nodes);

        // Set FinishJump State Nodes
        nodes = new List<string> { fsmList[2], fsmList[4], fsmList[6] };
        fsm[fsmList[3]] = new List<string>(nodes);

        // Set Falling State Nodes
        nodes = new List<string> { fsmList[2], fsmList[5], fsmList[6] };
        fsm[fsmList[4]] = new List<string>(nodes);

        // Set Landing State Nodes
        nodes = new List<string> { fsmList[0], fsmList[1], fsmList[6] };
        fsm[fsmList[5]] = new List<string>(nodes);


        List<string> mobFsmList = new List<string>()
        {
            "Idle", "MobMove", "MobAttack",
        };

        fsm[mobFsmList[0]].AddRange(
            new List<string>{
            mobFsmList[1], mobFsmList[2],
        }); 

        fsm[mobFsmList[1]] = new List<string>
        {
            mobFsmList[0], mobFsmList[2],
        };

        fsm[mobFsmList[2]] = new List<string>
        {
            mobFsmList [0], mobFsmList[1],
        };
    }

    public List<string> getList(string str)
    {
        return fsm[str];
    }
    
}
