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
    public float alertDistance;
    public float patrolDistance;
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
            if (GameObject.Find("Player").transform.position.x > transform.position.x)
            {
                dirX = 1;
                if ((GameObject.Find("Player").transform.position.x - transform.position.x) > 0.5f)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            if (GameObject.Find("Player").transform.position.x < transform.position.x)
            {
                dirX = -1;
                if((transform.position.x - GameObject.Find("Player").transform.position.x) > 0.5f)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }                
            }
            if (GameObject.Find("Player").transform.position.y > transform.position.y)
            {
                dirY = 1;
            }
            else
            {
                dirY = -1;
            }
        }
            transform.Translate(transform.right * dirX * Time.deltaTime);
            transform.Translate(transform.up * dirY * Time.deltaTime * 0.8f);
             
    }

    private void EnemyAttack()
    {
        float playerPositionX = GameObject.Find("Player").transform.position.x;
        float playerPositionY = GameObject.Find("Player").transform.position.y;
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
            animator.SetBool("attack",true);
            Invoke("EnemyBomb", 0.56f);
            Invoke("EnemyDie", 1.3f);
        }
    }

    private void EnemyBomb()
    {
        explosionCollider.enabled = true;
    }
    private void EnemyDie()
    {
        animator.SetTrigger("Die");
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(explosionCollider);
        Destroy(gameObject, 1f);
        isDie = true;
    }
}
