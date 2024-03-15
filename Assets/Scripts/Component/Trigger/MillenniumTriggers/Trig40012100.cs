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
            if (trigText.scripts[i].script.Equals("�Ͼ�ڸ��� �̾������� �Ƹ���, �׽�Ʈ�� ���� ������ ����� ���¾�"))
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
            npc.SetScript("���� ���� ������ ����� �ı���");
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
            npc.SetScript("���� �츮�� ���� ���� ���̾� ���� Ȯ������");
            await npc.Say();

            npc.SetScript("������ �̹� �༮�� �� ��ٷο� �ž�");
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
            npc.SetScript("���� ���� ����̾� �������� Ȯ������");

            GameManager.CameraManager.player = npc.transform;

            await npc.Say();

            npc.SetScript("���� ���� Ư�� �ɷ��� �����߾�");
            await npc.Say();

            GameManager.Progress.isActiveSkill = true;

            npc.SetScript("����Ű�� ������ �ִ� ������ ���� ������ �� �� �־�");
            await npc.Say();

            npc.SetScript("�� ���پ�� �Ƹ���");
            await npc.Say();

            GameManager.CameraManager.player = player.transform;

            player.canMove = true;
            go.GetComponent<Mob>().canMove = true;
            GameManager.Destroy(npc.gameObject);
            GameManager.ChangeUIState(UIState.InPlay);


        }*/
}
