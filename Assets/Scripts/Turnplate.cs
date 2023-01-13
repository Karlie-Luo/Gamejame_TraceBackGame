using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnplate : MonoBehaviour
{
    public Vector2 circleCentre;
    public float circleSpeed;
    private Vector2 axis;
    // Start is called before the first frame update
    void Start()
    {
        axis = new Vector2(-1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float posX;
        float posY;
        //transform.RotateAround(circleCentre, axis, 20 * Time.deltaTime);
        transform.Translate(axis * Time.deltaTime * circleSpeed);
        posX = transform.position.x - circleCentre.x;
        posY = transform.position.y - circleCentre.y;
        axis = new Vector2(posY, -posX);
    }
}
