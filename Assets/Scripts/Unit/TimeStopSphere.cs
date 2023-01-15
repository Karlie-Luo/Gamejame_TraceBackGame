using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopSphere : MonoBehaviour
{
    public float growspeed = 1f;
    public float totalTime;
    private float time = 0;
    public bool isChoosen;
    private void Update()
    {
        if (!isChoosen)
        {
            Debug.Log("bbbbb");
            this.gameObject.transform.localScale += new Vector3(growspeed * Time.unscaledDeltaTime, growspeed * Time.unscaledDeltaTime, 0);
        }
        else
        {
            Shrink(1);
        }
        if (time > 1.5)
        {
            Debug.Log("bbasdasdas");
            Shrink(3);
        }
        time += Time.unscaledDeltaTime;
    }

    private void OnDisable()
    {
        this.gameObject.transform.localScale = Vector3.zero;
        time = 0;
    }
    private void OnEnable()
    {
        Time.timeScale = 0;
        this.gameObject.transform.localScale = Vector3.zero;
    }
    public void Shrink(int times)
    {
        Debug.Log(" ’Àı");
        this.gameObject.transform.localScale -= new Vector3(times*growspeed * Time.unscaledDeltaTime, times*growspeed * Time.unscaledDeltaTime, 0);
        if (transform.localScale.x<1)
        {
            Time.timeScale = 1;
            this.isChoosen = false;
            this.gameObject.SetActive(false);
            time = 0;
        }
    }
}
