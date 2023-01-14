using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragController : MonoBehaviour
{

    public float decreasedPushSpeed;
    public float decreasedDragSpeed;
    public float formerSpeed;
    bool dragPressed = false;
    // Update is called once per frame
    GameObject dragObj = null;
    private Vector3 distance;

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
        if (dragObj != null && dragPressed&&!Player.instance.cannotDrag)
        {
            Debug.Log("hahaha");
            dragObj.transform.position = this.transform.position + distance;
            if (Player.instance.rb.velocity.x * distance.x < 0)
            {
                Player.instance.speed = decreasedDragSpeed;
            }
            else
            {
                Player.instance.speed = decreasedPushSpeed;
            }
        }
        else
        {
            Player.instance.speed = formerSpeed;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer==3)
        {
            if (dragPressed == true&&Player.instance.isGround)
            {
                dragObj = collision.gameObject;
                distance = collision.gameObject.transform.position - this.gameObject.transform.position;
                Debug.Log(dragObj.name);
            }
            else
            {
                //collision.gameObject.transform.parent = null;
                dragObj = null;
                distance = Vector3.zero;
            }
        }
    }

}
