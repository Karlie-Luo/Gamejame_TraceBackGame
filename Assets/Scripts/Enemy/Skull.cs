using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skull : MonoBehaviour
{
    private Animator animator;
    private int dirX = 1;
    private int dirY = 1;
    private bool isDie = false;
    private bool findPlayer = false;
    private PolygonCollider2D explosionCollider;
    private float startPosX;
    private float startPosY;
    private bool isBomb = false;
    public GameObject BombCollider;
    public float alertDistance;
    public float patrolDistance;
    public AudioSource dieAudio;
    public AudioSource boomAudio;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        explosionCollider = GetComponent<PolygonCollider2D>();
        startPosX = transform.position.x;
        startPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDie)
        {
            return;
        }
        EnemyMove();
        EnemyAttack();
    }

    private void EnemyMove()
    {
        System.Random xORy = new System.Random();
        if (findPlayer == false)
        {
            if (transform.position.x > startPosX + patrolDistance)
            {
                dirX = dirX * -1;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (transform.position.x <= startPosX - patrolDistance)
            {
                dirX = dirX * -1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (transform.position.y > startPosY + patrolDistance)
            {
                dirY = dirY * -1;
            }
            if (transform.position.y <= startPosY - patrolDistance)
            {
                dirY = dirY * -1;
            }
        }
        else
        {
            if (Player.Instance.transform.position.x > transform.position.x)
            {
                dirX = 1;
                if ((Player.Instance.transform.position.x - transform.position.x) > 0.5f)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            if (Player.Instance.transform.position.x < transform.position.x)
            {
                dirX = -1;
                if((transform.position.x - Player.Instance.transform.position.x) > 0.5f)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }                
            }
            if (Player.Instance.transform.position.y > transform.position.y)
            {
                dirY = 1;
            }
            else
            {
                dirY = -1;
            }
        }
            transform.Translate(transform.right * dirX * Time.deltaTime * 2f);
            transform.Translate(transform.up * dirY * Time.deltaTime * 2f);
             
    }

    private void EnemyAttack()
    {
        float playerPositionX = Player.Instance.transform.position.x;
        float playerPositionY = Player.Instance.transform.position.y;
        float distance = Mathf.Sqrt(Mathf.Abs(playerPositionX - transform.position.x) * Mathf.Abs(playerPositionX - transform.position.x) +
        Mathf.Abs(playerPositionY - transform.position.y) * Mathf.Abs(playerPositionY - transform.position.y));
        if (distance < alertDistance)
        {
            animator.SetBool("findPlayer", true);
            findPlayer = true;
        }else
        {
            animator.SetBool("findPlayer", false);
            findPlayer = false;
        }
        if (distance < 1f && !isBomb)
        {
            isBomb = true;
            boomAudio.Play();
            Invoke("EnemyBomb", 1.3f);
            Invoke("EnemyDie", 3f);
        }
    }

    private void EnemyBomb()
    {
        animator.SetBool("attack", true);
        Instantiate(BombCollider, transform.position, Quaternion.Euler(0f, 0f, 0f));
    }
    private void EnemyDie()
    {
        dieAudio.Play();
        animator.SetTrigger("Die");
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(explosionCollider);
        Destroy(gameObject, 1f);
        isDie = true;
    }
}
