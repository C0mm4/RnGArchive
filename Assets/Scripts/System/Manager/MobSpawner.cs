using System;
using System.Collections;
using System.Collections.Generic;
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

    public GameObject MobSpawn(string id, Vector3 pos)
    {
        GameObject ret = GameManager.InstantiateAsync(id, pos);
        ret.GetComponent<Mob>().CreateHandler();
        
        return ret;
    }

    public async Task<GameObject> BossSpawn(string id, Vector3 pos)
    {
        GameObject warning = GameManager.InstantiateAsync("BossAlert");
        Func.SetRectTransform(warning);
        await warning.GetComponent<BossAlter>().ShowWarning();

        return MobSpawn(id, pos);
        
    }

    public NPC NPCSpawn(string id, Vector3 pos)
    {
        GameObject ret = GameManager.InstantiateAsync(id, pos);
        ret.GetComponent<NPC>().npcId = id;
        return ret.GetComponent<NPC>();
    }
}
