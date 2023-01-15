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
    public Animator animt;
    public AudioSource walkAudio;
    public AudioSource jumpAudio;

    public float jumpForce,speed;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround, isJump;
    public bool cannotDrag;
    
    bool jumpPressed;
    bool jumpContinue;
    public int jumpCount;
    private int movingcount = 0;
    public float jumpDownForce;
    

    bool dragPressed = false;
    bool isTimeStopStart = false;
    bool isMovingAudioPlaying;

    float time;
    float timeStopTime;

    public GameObject sceneFadeInOut;
    public GameObject timestopsphere;

    public SpriteRenderer renderer;
    private float maxBlinkTime = 0.5f;
    private float blinkTime = 0f;
    public bool playerDeath = false;
    public bool afterBlink = false;
    private int blinkCount = 0;

    public static Player instance;
    public static Player Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (Player)this;
            DontDestroyOnLoad(this);
        }

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animation>();
        animt = GetComponent<Animator>();
        //audio = GetComponent<AudioSource>();
        walkAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDeath)
        {
            Debug.Log("doblink");
            DoBlink();
            return;
        }
        if (!isTimeStopStart)
        {
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
            {
                jumpPressed = true;
                jumpContinue = true;
                time = Time.time;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                jumpContinue = false;
            }
        }
        else 
        {
  
            timeStopTime += Time.unscaledDeltaTime;
            if (timeStopTime >= 2.5f)
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

    public void TimeStopChecks()
    {
        //Time.timeScale = 0;
        timestopsphere.gameObject.SetActive(true);
        timestopsphere.gameObject.transform.position = this.gameObject.transform.position + new Vector3(0, 0, 3);
        isTimeStopStart = true;
    }
    void GroundMove()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            if (movingcount==0&&isGround)
            {
                walkAudio.Play();
                movingcount++;
            }
            transform.localScale = new Vector3(horizontalMove * 5, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            movingcount = 0;
            walkAudio.Stop();
        }
        if (!isGround)
        {
            walkAudio.Stop();
        }
        animt.SetFloat("walk", Mathf.Abs(rb.velocity.x));
    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
            cannotDrag = false;
        }
        if (isGround && jumpPressed)
        {
            isJump = true;
            cannotDrag = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpAudio.Play();
        }
        animt.SetFloat("jumpFall", rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 3)
        {
            ColliderTest(true, collision);
        }
        if (isJump)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6||collision.gameObject.layer ==3)
        {
            ColliderTest(false, collision);
        }

    }

    public TransformStep GetTransfomStep()
    {
        TransformStep step = new TransformStep();
        step.position = transform.position;
        step.rotation = transform.rotation;
        return step;
    }
    public void Rebirth()
    {
        Debug.Log("Rebirth");
        playerDeath = true;
        //sceneFadeInOut.GetComponentInChildren<SceneFadeInOut>().ReloadEffect();
    } 

    public void DoBlink()
    {
        blinkTime += Time.deltaTime;
        blinkCount++;
        if(blinkTime >= maxBlinkTime)
        {
            blinkTime = 0f;
            renderer.enabled = true;
            playerDeath = false;
            afterBlink = true;
            TBManager.instance.PlayerRebirth();
            return;
        }
        if(blinkCount >= 20)
        {
            blinkCount = 0;
            renderer.enabled = !renderer.enabled;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Bomb")
        {
            Rebirth();
        }
    }

    private void ColliderTest(bool Ground,Collider2D collision)
    {
        Vector2 v = collision.ClosestPoint(this.transform.position);
        if (v.y < transform.position.y && Mathf.Abs(v.x - transform.position.x) < 0.1)
        {
            isGround = Ground;
            animt.SetBool("isGround", Ground);
        }
    }
}

