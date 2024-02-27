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

        npc.SetScript("정신이 들어 아리스?");

        await npc.Say();

        npc.SetScript("정신을 차렸나 보네");
        await npc.Say();

        npc.SetScript("아리스, 잠깐의 테스트를 위해 드론을 보냈어.");
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
        npc.SetScript("너의 빛의 검으로 드론을 파괴해");
        await npc.Say();
        npc.SetScript("");

        player.canMove = true;
        GameManager.UIManager.ChangeState(UIManager.UIState.InPlay);
    }
}
