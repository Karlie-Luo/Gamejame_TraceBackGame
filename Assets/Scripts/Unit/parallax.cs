using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private Camera cam;
    private Transform subject;

    Vector2 startPosition;

    float startZ;


    Vector2 travel => (Vector2)cam.transform.position - startPosition;

    float distanceFromsubject => transform.position.z - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromsubject > 0 ? cam.farClipPlane : cam.nearClipPlane));


    float parallaxFactor => Mathf.Abs(distanceFromsubject) / clippingPlane;

    // Start is called before the first frame update
    public void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        subject = Player.Instance.transform;
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    public void Update()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
