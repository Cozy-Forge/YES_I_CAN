using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CanController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    private float ForcePower = 10;
    [SerializeField] private Camera cam;
    public float JumpCool;
    public GameObject JumpEffect;
    public Transform EffectPos;

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && JumpCool >=2)
        {
            Vector3 forwardDirection = cam.transform.forward + Vector3.up;
            rb.AddForce(forwardDirection * ForcePower, ForceMode.VelocityChange);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10);
            JumpCool = 0;
            Instantiate(JumpEffect, EffectPos.position, Quaternion.identity);
        }

        JumpCool += Time.deltaTime;

    }


}
