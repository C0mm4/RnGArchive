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
        nodes = new List<string> { fsmList[0], fsmList[2], fsmList[4], fsmList[6] };
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
            //0          1           2               3               4                 5           6               7           8
            "MobIdle", "MobMove", "MobPrepareJump", "MobJumpFinish", "MobFalling", "MobLanding", "PrepareAttack", "MobAttack", "Pattern"
        };

        // Set Mob Idle State Nodes
        List<string> mobFsmNodes = new List<string>() { mobFsmList[1], mobFsmList[2], mobFsmList[4], mobFsmList[8] };
        fsm[mobFsmList[0]] = mobFsmNodes;

        // Set Mob Move State Nodes
        mobFsmNodes = new List<string>() { mobFsmList[0], mobFsmList[2], mobFsmList[4] , mobFsmList[6], mobFsmList[8] };
        fsm[mobFsmList[1]] = mobFsmNodes;

        // Set Mob PrepareJump State Nodes
        mobFsmNodes = new List<string>() { mobFsmList[3] };
        fsm[mobFsmList[2]] = mobFsmNodes;

        // Set Mob JumpFinish State Nodes
        mobFsmNodes = new List<string>() { mobFsmList[4] };
        fsm[mobFsmList[3]] = mobFsmNodes;

        // Set Mob Falling State Nodes
        mobFsmNodes = new List<string>() { mobFsmList[5] };
        fsm[mobFsmList[4]] = mobFsmNodes;

        // Set Mob Landing State Nodes
        mobFsmNodes = new List<string>() { mobFsmList[0] , mobFsmList[1] };
        fsm[mobFsmList[5]] = mobFsmNodes;

        // Set Mob PrepareAttack State Nodes
        mobFsmNodes = new List<string>() { mobFsmList[7] };
        fsm[mobFsmList[6]] = mobFsmNodes;

        // Set Mob Attack State Nodes
        mobFsmNodes = new List<string>() { mobFsmList[0], mobFsmList[7] };
        fsm[mobFsmList[7]] = mobFsmNodes;


    }

    public List<string> getList(string str)
    {
        return fsm[str];
    }
    
}
