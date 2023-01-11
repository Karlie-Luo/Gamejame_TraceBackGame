using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Bee : MonoBehaviour
{
    private Animator animator;
    private bool isDie = false;
    private bool shootFlag = false;
    public GameObject bullet;
    private float startTime;
    private int shootIndex = 0;
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
        EnemyMove();
        EnemyAttack();
        if(shootIndex >= 5)
        {
            animator.SetTrigger("die");
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject, 1f);
            isDie = true;
        }
    }

    private void EnemyMove()
    {
        float distance = Mathf.Abs(GameObject.Find("Player").transform.position.x - transform.position.x);
        if(distance < 0.5f)
        {
            if(Time.time - startTime > 5.0f)
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
            if(Time.time -startTime > 0.5f)
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0));
                shootFlag = false;
                shootIndex += 1;
            }            
        }
    }
}
