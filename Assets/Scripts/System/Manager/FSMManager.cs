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
            // 0    1       2       3       4           5           6           7               8
            "Idle", "Move", "Shot", "Jump", "MoveShot", "JumpSit", "JumpShot", "JumpSitShot", "SpecialMove"
        };

        // Set Idle State Nodes
        List<string> nodes = new List<string> { fsmList[1], fsmList[2], fsmList[3], fsmList[8] };
        fsm[fsmList[0]] = new List<string>(nodes);

        // Set Move State Nodes
        nodes = new List<string> { fsmList[3], fsmList[4], fsmList[8], fsmList[0] };
        fsm[fsmList[1]] = new List<string>(nodes);

        // Set Shot State Nodes
        nodes = new List<string> { fsmList[4], fsmList[3], fsmList[0], fsmList[6] };
        fsm[fsmList[2]] = new List<string>(nodes);

        // Set Jump State Nodes
        nodes = new List<string> { fsmList[5], fsmList[6], fsmList[7], fsmList[0] };
        fsm[fsmList[3]] = new List<string>(nodes);


        // Set MoveShot
        nodes = new List<string> { fsmList[0], fsmList[2], fsmList[6] };
        fsm[fsmList[4]] = new List<string>(nodes);
        // Set JumpShot, JumpSitShot State Nodes
        nodes = new List<string> { fsmList[0], fsmList[2], fsmList[6] };
        fsm[fsmList[6]] = new List<string>(nodes);
        fsm[fsmList[7]] = new List<string>(nodes);
        nodes = new List<string> { fsmList[0] };
        fsm[fsmList[8]] = new List<string>(nodes);

        // Set JumpSit State Nodes
        nodes = new List<string> { fsmList[7] , fsmList[0] };
        fsm[fsmList[5]] = new List<string>(nodes);

    }

    public List<string> getList(string str)
    {
        return fsm[str];
    }
    
}
