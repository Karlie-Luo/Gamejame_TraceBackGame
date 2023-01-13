using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragController : MonoBehaviour
{

    bool dragPressed = false;
    // Update is called once per frame
    GameObject dragObj = null;
    private float distance;
    float x;
    private void Start()
    {
        x = this.transform.position.x;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            dragPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            dragPressed = false;
        }
    }

    private void FixedUpdate()
    {
        if (dragObj != null&&dragPressed)
        {
            Debug.Log("hahaha");
            dragObj.transform.position = this.transform.position + new Vector3(distance, 0, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer==3)
        {
            Debug.Log("aaaaaa");
            Debug.Log(dragPressed + "dragPressed");
            if (dragPressed == true)
            {
                dragObj = collision.gameObject;
                distance = collision.gameObject.transform.position.x - this.gameObject.transform.position.x;
                Debug.Log(dragObj.name);
            }
            else
            {
                //collision.gameObject.transform.parent = null;
                dragObj = null;
                distance = 0;
            }
        }
    }

}
