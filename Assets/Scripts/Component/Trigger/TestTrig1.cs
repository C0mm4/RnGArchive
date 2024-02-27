using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestTrig1 : Trigger
{
    public Transform utahaSpawn;
    public Door Door;

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

        var pos = Door.transform.position;
        pos.x += Door.InDir.x * 1.5f * Door.transform.localScale.x;

        GameObject go = GameManager.InstantiateAsync("Sweaper", pos);
        go.GetComponent<Mob>().InDoor(Door);

        await Task.Delay(TimeSpan.FromSeconds(0.5f));

        go = GameManager.InstantiateAsync("Sweaper", pos);
        go.GetComponent<Mob>().InDoor(Door);

        await Task.Delay(TimeSpan.FromSeconds(0.5f));

        go = GameManager.InstantiateAsync("Sweaper", pos);
        go.GetComponent<Mob>().InDoor(Door);

        await Task.Delay(TimeSpan.FromSeconds(0.5f));

        go = GameManager.InstantiateAsync("Sweaper", pos);
        go.GetComponent<Mob>().InDoor(Door);

        await Task.Delay(TimeSpan.FromSeconds(0.5f));

        await GameManager.CameraManager.CameraMoveV2V(cameraMoveStart.position, cameraMoveEnd.position);

        GameManager.CameraManager.CameraMove(player.transform);
        npc.SetScript("���� ���� ������ ����� �ı���");
        await npc.Say();
        npc.SetScript("");

        player.canMove = true;
        GameManager.UIManager.ChangeState(UIManager.UIState.InPlay);
    }
}
