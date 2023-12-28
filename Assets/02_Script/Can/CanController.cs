using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    private float ForcePower = 10;
    [SerializeField] private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 forwardDirection = cam.transform.forward;
            rb.AddForce(forwardDirection * ForcePower, ForceMode.VelocityChange);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10);
        }
    }
}
