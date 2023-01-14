using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject bullet;
    private bool isDie = false;
    private Animator animator;
    private float startTime;
    public bool left;
    public float shootInterval;
    public float alertDistance;
    public float dieSpeed;
    public AudioSource dieAudio;
    public AudioSource shootAudio;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDie)
        {
            return;
        }
        FindPlayer();
    }

    private void FindPlayer()
    {
        float distance = Mathf.Abs(GameObject.Find("Player").transform.position.x - transform.position.x);
        bool isSameHeight = false;
        if (Mathf.Abs(GameObject.Find("Player").transform.position.y - transform.position.y) <= 3f)
        {
            isSameHeight = true;
        }
        if(distance < alertDistance && isSameHeight)
        {            
            if (Time.time - startTime > shootInterval)
            {
                animator.SetBool("Attack", true);
                Invoke("Fire", 0.3f);
                startTime = Time.time;
            }
        }
        if (distance >= alertDistance || !isSameHeight)
        {
            animator.SetBool("Attack", false);
        }
    }

    private void Fire()
    {
        shootAudio.Play();
        float shiftPos = 0.5f;
        float shiftAngle = 0;
        if(left)
        {
            shiftPos = - 0.5f;
            shiftAngle = 180;
        }
        Vector2 bulletPosition = new Vector2(transform.position.x + shiftPos, transform.position.y + 0.4f);
        Instantiate(bullet, bulletPosition, Quaternion.Euler(0, 0, shiftAngle));
    }
    
    private void EnemyDie()
    {
        dieAudio.Play();
        animator.SetTrigger("Die");
        isDie = true;
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Vector2 vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            float velAbs = Mathf.Sqrt(vel.x * vel.x + vel.y * vel.y);
            if (velAbs > dieSpeed)
            {
                EnemyDie();
            }
        }
        if (collision.gameObject.tag == "Bomb")
        {
            EnemyDie();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            EnemyDie();
            Debug.Log($"BecauseBomb");
        }
    }
}
