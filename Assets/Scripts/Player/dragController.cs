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
            Debug.Log("玩家位置" + this.transform.position.x+" distance"+distance.x);
            
            dragObj.transform.position = this.transform.position + distance;
            Debug.Log("物体位置"+dragObj.transform.position.x);
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
            if (dragPressed == true&&Player.instance.isGround&&count == 0)
            {
                count++;
                dragObj = collision.gameObject;
                distance = collision.gameObject.transform.position - this.gameObject.transform.position;
                Debug.Log(dragObj.name);
                Debug.Log("dddd");
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
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == 3)
    //    {
    //        dragObj = null;
    //        distance = Vector3.zero;
    //    }
    //}

}
