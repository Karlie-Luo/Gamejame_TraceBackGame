using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.Instance.afterBlink)
        {
            Debug.Log("move place");
            Player.Instance.transform.position = this.transform.position;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Player.Instance.afterBlink = false;
        }
    }

}
