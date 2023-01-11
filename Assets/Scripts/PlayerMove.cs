using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public Collider2D coll;
    public Animation anim;

    public float jumpForce,speed;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround, isJump;
     
    bool jumpPressed;
    bool jumpContinue;
    public int jumpCount;

    public float jumpDownForce;

    bool dragPressed = false;

    float time;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&jumpCount>0)
        {
            jumpPressed = true;
            jumpContinue = true;
            time = Time.time;
            Debug.Log("aaaaaa");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpContinue = false;
        }
        
    
       
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        GroundMove();
        Jump();
        if (rb.velocity.y < 0||(rb.velocity.y>0&&rb.velocity.y<10))
        {
            rb.AddForce(-Vector2.up*jumpDownForce);
        }
        if (jumpContinue&&(Time.time-time)<2)
        {
            rb.AddForce(new Vector2(0, 40*(2-Time.time+time)));
        }
    }

    void GroundMove()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    void Jump()
    {
        Debug.Log(isGround+"isGround");
        Debug.Log(jumpPressed + "jumpPressed");

        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (isGround && jumpPressed)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        //else if(!isGround&&jumpPressed)
        //{ }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }


}
