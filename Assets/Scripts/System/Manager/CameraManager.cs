using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;



    public void Awake()
    {
        GameObject playerGo= GameObject.FindWithTag("Player");
        if (playerGo)
        {
            player = playerGo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            try
            {
                player = GameObject.FindWithTag("Player");
            }
            catch
            {

            }
        }
        else
        {

        }
    }
}
