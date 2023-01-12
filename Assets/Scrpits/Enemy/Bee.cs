using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Bee : MonoBehaviour
{
    private Animator animator;
    private bool isDie = false;
    private bool shootFlag = false;
    public GameObject bullet;
    private float startTime;
    private int shootIndex = 0;
    public float shootInterval;
    public float alertDistance;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (shootIndex >= 5)
        {
            EnemyDie();
        }
        if (isDie)
        {
            return;
        }        
        EnemyMove();
        EnemyAttack();       
    }

    private void EnemyMove()
    {
        float distance = Mathf.Abs(GameObject.Find("Player").transform.position.x - transform.position.x);
        if(distance < alertDistance)
        {
            if (GameObject.Find("Player").transform.position.x > transform.position.x)
            {
                transform.Translate(transform.right  * Time.deltaTime);
            }
            if (GameObject.Find("Player").transform.position.x < transform.position.x)
            {
                transform.Translate(transform.right * -1 * Time.deltaTime);
            }
        }
        if(distance < 0.2f)
        {
            if (Time.time - startTime > shootInterval)
            {
                shootFlag = true;
                startTime = Time.time;
            }
        }
    }
    private void EnemyAttack()
    {        
        if(shootFlag)
        {
            animator.SetTrigger("attackNow");
            Invoke("Fire", 0.8f);
            shootFlag = false;
            shootIndex += 1;
        }
    }

    private void Fire()
    {
        Vector2 bulletPosition = new Vector2(transform.position.x, transform.position.y - 1.0f);
        Instantiate(bullet, bulletPosition, Quaternion.Euler(0, 0, 0));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            EnemyDie();
        }
    }
    private void EnemyDie()
    {
        animator.SetTrigger("die");
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(gameObject, 1f);
        isDie = true;
    }
}
