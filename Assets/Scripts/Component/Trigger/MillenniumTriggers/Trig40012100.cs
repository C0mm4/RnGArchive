using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class Trig40012100 : Trigger
{
    public Transform utahaSpawn;
    public Door Door;

    public Trigger nextTrigger;

    public override async Task Action()
    {
        PlayerController player = GameManager.Player.GetComponent<PlayerController>();
        NPCSpawn("20001001", utahaSpawn);
        for(int i = 0; i < trigText.scripts.Count; i++)
        {
            string npcId = trigText.scripts[i].npcId;
            if (npcId.Equals("90000000"))
            {
                List<Script> selections = new List<Script>();
                selections.Add(trigText.scripts[i]);
                if(i != trigText.scripts.Count - 1)
                {
                    for (int j = i + 1; j < trigText.scripts.Count && trigText.scripts[j].npcId.Equals("90000001"); j++)
                    {
                        selections.Add(trigText.scripts[j]);
                        i++;
                    }
                }
                await GenSelectionUI(selections);
            }
            else
            {
                NPC targetNPC = FindNPC(npcId);
                await NPCSay(trigText.scripts[i], targetNPC);
            }
            if (trigText.scripts[i].script.Equals("일어나자마자 미안하지만 아리스, 테스트를 위해 전투용 드론을 보냈어"))
            {
                GameManager.CameraManager.CameraMove(Door.transform);
                var pos = Door.transform.position;
                pos.x += Door.InDir.x * 1.5f * Door.transform.localScale.x;

#pragma warning disable CS4014
                GameObject[] gos = new GameObject[4];
                GameObject go;
                for (int k = 0; k < 4; k++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1f));
                    go = GameManager.MobSpawner.MobSpawn("Sweaper", pos);
                    Vector3 movePos = Door.transform.position;
                    movePos.x += -Door.InDir.x * ((Door.InDir.x * 1.25f) + (i * 0.9f));
                    gos[k] = go;
                    nextTrigger.GetComponent<Trig40012101>().gameObjects.Add(go);
                }
#pragma warning restore CS4014
                await Task.Delay(TimeSpan.FromSeconds(1f));

                GameManager.CameraManager.CameraMove(player.transform);
            }
        }
    }

    public override bool AdditionalCondition()
    {
        return true;
    }
    /*
        public override async Task TriggerActive()
        {
            await base.TriggerActive();
            PlayerController player = GameManager.player.GetComponent<PlayerController>();
            GameManager.ChangeUIState(UIState.CutScene);
            player.canMove = false;

            NPC npc = GameManager.MobSpawner.NPCSpawn("TestNPC", utahaSpawn.position);

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
            var pos = Door.transform.position;
            pos.x += Door.InDir.x * 1.5f * Door.transform.localScale.x;

            #pragma warning disable CS4014
            GameObject[] gos = new GameObject[4];
            GameObject go;
            for(int i = 0; i < 4; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(1f));
                go = GameManager.MobSpawner.MobSpawn("Sweaper", pos);
                Vector3 movePos = Door.transform.position;
                movePos.x += -Door.InDir.x * ((Door.InDir.x * 1.25f) + (i * 0.9f)); 
                gos[i] = go;
            }
            #pragma warning restore CS4014
            await Task.Delay(TimeSpan.FromSeconds(1f));

            GameManager.CameraManager.CameraMove(player.transform);
            npc.SetScript("너의 빛의 검으로 드론을 파괴해");
            await npc.Say();
            npc.SetScript("");
            foreach(GameObject goa in gos)
            {
                goa.GetComponent<Mob>().canMove = true;
            }
            player.canMove = true;
            GameManager.Destroy(npc.gameObject);
            GameManager.ChangeUIState(UIState.InPlay);

            while (true)
            {
                var Objs = GameObject.FindGameObjectsWithTag("Enemy");
                if(Objs.Length > 0)
                {
                    await Task.Yield();
                }
                else
                {
                    break;
                }
            }

            GameManager.ChangeUIState(UIState.CutScene);
            player.canMove = false;

            npc = GameManager.MobSpawner.NPCSpawn("TestNPC", utahaSpawn.position);
            npc.SetScript("역시 우리가 만든 빛의 검이야 성능 확실하지");
            await npc.Say();

            npc.SetScript("하지만 이번 녀석은 좀 까다로울 거야");
            await npc.Say();

            go =  await GameManager.MobSpawner.BossSpawn("SweaperBoss", pos);

            player.canMove = true;
            go.GetComponent<Mob>().canMove = true;
            GameManager.Destroy(npc.gameObject);
            GameManager.ChangeUIState(UIState.InPlay);

            while (true)
            {
                if (go.GetComponent<Mob>().status.currentHP <= 15)
                {
                    break;
                }
                else
                {
                    await Task.Yield();
                }
            }
            GameManager.ChangeUIState(UIState.CutScene);
            player.canMove = false;
            go.GetComponent<Mob>().canMove = false;

            npc = GameManager.MobSpawner.NPCSpawn("TestNPC", utahaSpawn.position);
            npc.SetScript("역시 신형 드론이야 내구도는 확실하지");

            GameManager.CameraManager.player = npc.transform;

            await npc.Say();

            npc.SetScript("빛의 검의 특수 능력을 개방했어");
            await npc.Say();

            GameManager.Progress.isActiveSkill = true;

            npc.SetScript("공격키를 누르고 있는 것으로 차지 공격을 할 수 있어");
            await npc.Say();

            npc.SetScript("자 날뛰어봐 아리스");
            await npc.Say();

            GameManager.CameraManager.player = player.transform;

            player.canMove = true;
            go.GetComponent<Mob>().canMove = true;
            GameManager.Destroy(npc.gameObject);
            GameManager.ChangeUIState(UIState.InPlay);


        }*/
}
