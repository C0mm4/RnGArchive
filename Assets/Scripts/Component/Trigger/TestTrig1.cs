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
        /*
                var awaitNpc = GameManager.InstantiateAsync("TestNPC", utahaSpawn.position);

                GameObject npcObj = awaitNpc;
                NPC npc = npcObj.GetComponent<NPC>();

                npc.SetScript("������ ��� �Ƹ���?");

                await npc.Say();

                SelectUI selectUI = GameManager.InstantiateAsync("SelectUI").GetComponent<SelectUI>();
                List<string> str = new List<string> {"�ý��� ���� �غ� �Ϸ�" };
                selectUI.CreateHandler(1, str);
                await selectUI.Select();


                npc.SetScript("������ ���ȳ� ����");
                await npc.Say();

                npc.SetScript("�Ƹ���, ����� �׽�Ʈ�� ���� ����� ���¾�.");
                await npc.Say();


                GameManager.CameraManager.CameraMove(Door.transform);
        */
        var pos = Door.transform.position;
        pos.x += Door.InDir.x * 1.5f * Door.transform.localScale.x;
#pragma warning disable CS4014
        GameObject[] gos = new GameObject[4];
        for(int i = 0; i < 4; i++)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
            GameObject go = GameManager.InstantiateAsync("Sweaper", pos);
            go.GetComponent<Mob>().CreateHandler();
            Vector3 movePos = Door.transform.position;
            movePos.x += -Door.InDir.x * ((Door.InDir.x * 1.25f) + (i * 0.9f)); 
            go.GetComponent<Mob>().InDoor(Door, movePos);
            gos[i] = go;
        }
#pragma warning restore CS4014
/*
        await Task.Delay(TimeSpan.FromSeconds(1f));

        GameManager.CameraManager.CameraMove(player.transform);
        npc.SetScript("���� ���� ������ ����� �ı���");
        await npc.Say();
        npc.SetScript("");
*/
        player.canMove = true;
        foreach(GameObject go in gos)
        {
            go.GetComponent<Mob>().canMove = true;
        }
        GameManager.UIManager.ChangeState(UIManager.UIState.InPlay);
    }
}
