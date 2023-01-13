using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckPoint : MonoBehaviour
{
    public GameObject player;
    public bool playerDie = false;
    private Vector3 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDie)
        {
            SceneFadeInOut.sceneReloading = true;
            SceneFadeInOut.sceneEnding = true;
            //player.transform.position = playerPos;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerPos = player.transform.position;
        }
    }

}
