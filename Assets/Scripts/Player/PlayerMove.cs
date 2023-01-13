using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kurisu.TimeControl;

public class Player : MonoBehaviour
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
    bool isTimeStopStart = false;

    float time;
    float timeStopTime;

    public GameObject timestopsphere;

    public static Player instance;
    public static Player Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimeStopStart)
        {
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
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
            TimeStopChecks();
        }
        else 
        {
            Debug.Log("时停开始");
            timeStopTime += Time.unscaledDeltaTime;
            if (timeStopTime >= 2.0f)
            {

                Time.timeScale = 1;
                Debug.Log(Time.timeScale);
                timestopsphere.SetActive(false);
                isTimeStopStart = false;
                timeStopTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isTimeStopStart)
        {
            //isGround = Physics2D.OverlapCircle(groundCheck.position, 0.01f, ground);
            GroundMove();
            Jump();
            if (rb.velocity.y < 0 || (rb.velocity.y > 0 && rb.velocity.y < 10))
            {
                rb.AddForce(-Vector2.up * jumpDownForce);
            }
            if (jumpContinue && (Time.time - time) < 2)
            {
                rb.AddForce(new Vector2(0, 40 * (2 - Time.time + time)));
            }
        }
    }

    private void TimeStopChecks()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("按下时停");
            Time.timeScale = 0;
            timestopsphere.gameObject.SetActive(true);
            timestopsphere.gameObject.transform.position = this.gameObject.transform.position + new Vector3(0, 0, 3);
            isTimeStopStart = true;
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
        //else if (jumpPressed && jumpCount > 0 && isJump)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //    jumpCount--;
        //    jumpPressed = false;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGround = true;
            Debug.Log("在地面上");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGround = false;
            Debug.Log("起飞");
        }
    }

    public TransformStep GetTransfomStep()
    {
        TransformStep step = new TransformStep();
        step.position = transform.position;
        step.rotation = transform.rotation;
        return step;
    }
}
