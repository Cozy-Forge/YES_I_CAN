using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private const float maxYVel =30;
    public float rotateSpeed = 0;

    private float xVel, yVel;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        xVel += Input.GetAxisRaw("Mouse X") * rotateSpeed * Time.deltaTime;
        yVel += Input.GetAxisRaw("Mouse Y") * rotateSpeed * Time.deltaTime;

        yVel = Mathf.Clamp(yVel, -maxYVel, maxYVel);
        var rotateVec = new Vector3(-yVel, xVel);
        transform.eulerAngles = rotateVec;


    }
}
