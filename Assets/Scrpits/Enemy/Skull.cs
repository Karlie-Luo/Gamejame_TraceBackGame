using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    private Animator animator;
    private bool isDie = false;
    private bool boomFlag = false;
    private PolygonCollider2D explosionCollider;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        explosionCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDie)
        {
            return;
        }
        EnemyMove();
    }

    private void EnemyMove()
    {
        float playerPositionX = GameObject.Find("Player").transform.position.x;
        float playerPositionY = GameObject.Find("Player").transform.position.y;
        float distance = Mathf.Sqrt(Mathf.Abs(playerPositionX - transform.position.x) * Mathf.Abs(playerPositionX - transform.position.x) +
        Mathf.Abs(playerPositionY - transform.position.y) * Mathf.Abs(playerPositionY - transform.position.y));
        if (distance < 4f)
        {
            animator.SetBool("findPlayer", true);
            if (GameObject.Find("Player").transform.position.x > transform.position.x)
            {
                transform.Translate(transform.right * Time.deltaTime);
            }else
            {
                transform.Translate(transform.right * -1 * Time.deltaTime);
            }
            if(GameObject.Find("Player").transform.position.y > transform.position.y)
            {
                transform.Translate(transform.up * Time.deltaTime);
            }else
            {
                transform.Translate(transform.up * Time.deltaTime * -1);
            }
        }
        if(distance < 1f)
        {
            boomFlag = true;
        }
    }

    private void EnemyAttack()
    {
        if(boomFlag)
        {
            animator.SetTrigger("attack");
        }
    }

    private void EnemyDie()
    {

    }
}
