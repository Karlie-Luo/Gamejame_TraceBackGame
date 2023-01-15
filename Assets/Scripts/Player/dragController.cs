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
    private int count = 0;

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
            dragObj.transform.position = this.transform.position + distance;
            Debug.Log("ŒÔÃÂŒª÷√"+dragObj.transform.position.x);
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
        if (!Player.instance.isGround)
        {
            dragObj = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer==3&& Player.instance.isGround)
        {
            if (dragPressed == true&&Player.instance.isGround&&count == 0&&!Player.instance.isBox)
            {
                count++;
                dragObj = collision.gameObject;
                distance = collision.gameObject.transform.position - this.gameObject.transform.position;
            }
            else if(!dragPressed||!Player.instance.isGround)
            {
                //collision.gameObject.transform.parent = null;
                dragObj = null;
                distance = Vector3.zero;
                count = 0;
            }
        }
    }
}
