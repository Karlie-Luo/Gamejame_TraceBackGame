using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    private Animator animator;
    private int dir = -1;
    private float startPosX;
    private bool isDie = false;
    private PolygonCollider2D spikesOutCollider;
    private bool findPlayer = false;
    public float alertDistance;
    public float patrolDistance;
    void Start()
    {
        spikesOutCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
        startPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDie)
        {
            return;
        }
        EnemyMove();
        //EnemyAttack();
    }

    private void EnemyMove()
    {
        if(findPlayer == false)
        {           
            if (transform.position.x > startPosX + patrolDistance)
            {
                dir = dir * -1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (transform.position.x < startPosX - patrolDistance)
            {
                dir = dir * -1;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            if(GameObject.Find("Player").transform.position.x > transform.position.x)
            {
                dir = 1;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (GameObject.Find("Player").transform.position.x < transform.position.x)
            {
                dir = -1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        transform.Translate(transform.right * dir * 0.5f * Time.deltaTime);
    }

    private void EnemyAttack()
    {
        bool hasSpikes = false;
        float distance = Mathf.Abs(GameObject.Find("Player").transform.position.x - transform.position.x);
        bool isSameHeight = false;
        if(Mathf.Abs(GameObject.Find("Player").transform.position.y - transform.position.y) <= 0.5f)
        {
            isSameHeight = true;
        }
        if (distance < alertDistance && isSameHeight)
        {
            animator.SetBool("findPlayer", true);
            spikesOutCollider.enabled = true;
            hasSpikes = true;
            findPlayer = true;

        }
        if(distance >= alertDistance || !isSameHeight)
        {
            animator.SetBool("findPlayer", false);
            if(hasSpikes)
            {
                spikesOutCollider.enabled = false;
                hasSpikes = false;
                findPlayer = false;
            }            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Quaternion quaternion = Quaternion.Euler(0, 0, 0);
        if (collision.gameObject.tag == "Player")
        {
            EnemyDie();
        }
        if(collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Player") //µØ°å
        {
            dir *= -1;
            if(transform.localRotation == quaternion)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void EnemyDie()
    {
        animator.SetTrigger("die");
        transform.position = new Vector2(transform.position.x, transform.position.y - 0.05f);
        isDie = true;
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(spikesOutCollider);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(gameObject, 0.5f);
    }
}
