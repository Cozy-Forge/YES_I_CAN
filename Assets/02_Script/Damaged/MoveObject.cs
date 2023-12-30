using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    Rigidbody _rb;
    Vector3 _dir;
    float _speed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Go(Vector3 dir, float speed)
    {
        _dir = dir;
        _speed = speed; 
    }

    void Update()
    {
        _rb.AddForce(_dir * _speed);
    }
}
