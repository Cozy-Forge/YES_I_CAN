using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageSelectPlayer : MonoBehaviour
{
    Camera _mainCam;

    private float _xRotate, _yRotate;
    public float _rotateSpeed = 500.0f;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        // 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.position += (_mainCam.transform.right * h + _mainCam.transform.forward * v).normalized * Time.deltaTime;


        // 회전
        float x = -Input.GetAxis("Mouse Y") * Time.deltaTime * _rotateSpeed;
        float y = Input.GetAxis("Mouse X") * Time.deltaTime * _rotateSpeed;

        _yRotate = transform.eulerAngles.y + y;
        //xRotate = transform.eulerAngles.x + xRotateMove; 
        _xRotate = _xRotate + x;

        _xRotate = Mathf.Clamp(_xRotate, -90, 90); // 위, 아래 고정

        transform.eulerAngles = new Vector3(_xRotate, _yRotate, 0);
    }
}
