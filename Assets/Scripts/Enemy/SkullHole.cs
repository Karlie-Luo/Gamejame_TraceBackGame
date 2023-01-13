using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullHole : MonoBehaviour
{
    private float startTime;
    public bool used = true;
    public GameObject Skull;
    public float generateInterval;
    public float alertDistance;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(!used)
        {
            return;
        }
         if(Time.time - startTime> generateInterval)
        {
            Gengrate();
            startTime = Time.time;
        }
    }

    private void Gengrate()
    {
        float playerPositionX = GameObject.Find("Player").transform.position.x;
        float playerPositionY = GameObject.Find("Player").transform.position.y;
        float distance = Mathf.Sqrt(Mathf.Abs(playerPositionX - transform.position.x) * Mathf.Abs(playerPositionX - transform.position.x) +
        Mathf.Abs(playerPositionY - transform.position.y) * Mathf.Abs(playerPositionY - transform.position.y));
        if(distance < alertDistance)
        {
            Instantiate(Skull, transform.position, Quaternion.Euler(0, 0, 0));
        }
    }
}
