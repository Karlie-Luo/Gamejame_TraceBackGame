using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopSphere : MonoBehaviour
{
    public float growspeed = 1f;
    public float totalTime;
    private float time = 0;
    private void Update()
    {
        Debug.Log(time);
        if (time < 1)
        {
            this.gameObject.transform.localScale += new Vector3(growspeed * Time.unscaledDeltaTime, growspeed * Time.unscaledDeltaTime, 0);
        }
        else if (time >= 1)
        {
            this.gameObject.transform.localScale -= new Vector3(growspeed * Time.unscaledDeltaTime, growspeed * Time.unscaledDeltaTime, 0);
        }
        if (2-time<0.01)
        {
            time = 0;
        }
        time += Time.unscaledDeltaTime;
    }

    private void OnDisable()
    {
        this.gameObject.transform.localScale = Vector3.zero;
    }
    private void OnEnable()
    {
        this.gameObject.transform.localScale = Vector3.zero;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
