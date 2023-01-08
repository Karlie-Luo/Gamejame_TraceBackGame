using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopSphere : MonoBehaviour
{
    public float growspeed = 1f;
    private void Update()
    {
        this.gameObject.transform.localScale += new Vector3(growspeed * Time.unscaledDeltaTime, growspeed * Time.unscaledDeltaTime, 0);
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
        Debug.Log("1");
    }
}
