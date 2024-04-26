using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            
            GameManager.MobSpawner.MobSpawn("dealmeter", GameManager.player.transform.position);
        }
        if (GameManager.Player != null) 
        {
            GameManager.Player.GetComponent<PlayerController>().isImmune = true;
        }

    }
}
