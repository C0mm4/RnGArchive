using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestTrig1 : Trigger
{
    public Transform utahaSpawn;

    public Transform cameraMoveStart;
    public Transform cameraMoveEnd;
    public override async Task TriggerActive()
    {
        await base.TriggerActive();
        PlayerController player = GameManager.player.GetComponent<PlayerController>();
        GameManager.UIManager.ChangeState(UIManager.UIState.CutScene);
        player.canMove = false;

        var awaitNpc = GameManager.InstantiateAsync("TestNPC", utahaSpawn.position);

        GameObject npcObj = awaitNpc;
        NPC npc = npcObj.GetComponent<NPC>();

        npc.SetScript("������ ��� �Ƹ���?");

        await npc.Say();

        npc.SetScript("������ ���ȳ� ����");
        await npc.Say();

        npc.SetScript("�Ƹ���, ����� �׽�Ʈ�� ���� ����� ���¾�.");
        await npc.Say();

        GameObject go = GameManager.InstantiateAsync("");
        await GameManager.CameraManager.CameraMoveV2V(cameraMoveStart.position, cameraMoveEnd.position);

        GameManager.CameraManager.CameraMove(player.transform);
        npc.SetScript("���� ���� ������ ����� �ı���");
        await npc.Say();
        npc.SetScript("");

        player.canMove = true;
        GameManager.UIManager.ChangeState(UIManager.UIState.InPlay);
    }
}
