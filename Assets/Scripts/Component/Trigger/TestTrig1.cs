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

                npc.SetScript("정신이 들어 아리스?");

                await npc.Say();

                SelectUI selectUI = GameManager.InstantiateAsync("SelectUI").GetComponent<SelectUI>();
                List<string> str = new List<string> {"시스템 가동 준비 완료" };
                selectUI.CreateHandler(1, str);
                await selectUI.Select();


                npc.SetScript("정신을 차렸나 보네");
                await npc.Say();

                npc.SetScript("아리스, 잠깐의 테스트를 위해 드론을 보냈어.");
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
        npc.SetScript("너의 빛의 검으로 드론을 파괴해");
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
