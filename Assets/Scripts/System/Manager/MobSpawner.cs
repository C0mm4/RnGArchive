using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MobSpawner
{
    public void Initialize()
    {
        // Test Area

    }

    public void Step()
    {

    }

    public GameObject MobSpawn(string id, Vector3 pos, bool isForceMove = false)
    {
        GameObject ret = GameManager.InstantiateAsync(id, pos);
        ret.GetComponent<Mob>().isForceMoving = isForceMove;
        ret.GetComponent<Mob>().CreateHandler();
        ret.transform.SetParent(GameManager.Instance.currentMapObj.transform);
        return ret;
    }

    public GameObject MobSpawn(string id, Transform trans)
    {
        GameObject ret = GameManager.InstantiateAsync(id, trans.position);
        ret.GetComponent<Mob>().CreateHandler();
        ret.transform.SetParent(GameManager.Instance.currentMapObj.transform);
        return ret;
    }

    public GameObject MobSpawn(string id, string spawnP)
    {
        GameObject ret = GameManager.InstantiateAsync(id, Func.FindSpawnP(spawnP).position);
        ret.GetComponent<Mob>().CreateHandler();
        ret.transform.SetParent(GameManager.Instance.currentMapObj.transform);

        return ret;

    }

    public async Task<GameObject> BossSpawn(string id, Vector3 pos)
    {
        GameObject warning = GameManager.InstantiateAsync("BossAlert");
        Func.SetRectTransform(warning);
        await warning.GetComponent<BossAlter>().ShowWarning();

        return MobSpawn(id, pos);
        
    }

    public async Task<GameObject> BossSpawn(string id, string trans)
    {
        GameObject warning = GameManager.InstantiateAsync("BossAlert");
        Func.SetRectTransform(warning);
        await warning.GetComponent<BossAlter>().ShowWarning();

        return MobSpawn(id, Func.FindSpawnP(trans));
    }

    public NPC NPCSpawn(string id, Vector3 pos)
    {
        var ids = id.Split('_');

        GameObject ret = GameManager.InstantiateAsync(id, pos);
        ret.GetComponent<NPC>().npcId = ids[0];
        ret.transform.SetParent(GameManager.Instance.currentMapObj.transform);
        GameManager.Stage.NPCScriptSet(ret.GetComponent<NPC>());
        GameManager.Stage.currentNPCs.Add(ret.GetComponent<NPC>());

        if (ids.Length == 2)
        {
            ret.GetComponent<PlayerTest>().gravityModifier = 0;
        }


        return ret.GetComponent<NPC>();
    }
}
