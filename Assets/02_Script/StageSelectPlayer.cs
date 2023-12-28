using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class StageSelectPlayer : MonoBehaviour
{
    private Camera _mainCam;
    private float _xRotate, _yRotate;

    [SerializeField] private float _rotateSpeed = 500.0f;
    [SerializeField] private float _speed = 5f;

    private void Awake()
    {
        _mainCam = Camera.main;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        Move();
        Rotate();
        Check();
    }

    private void Check()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out hitInfo, _mainCam.farClipPlane, 1 << 10))
        {
            if (hitInfo.transform.TryGetComponent<StageObject>(out StageObject so))
            {
                so.RemainTime = 0.3f;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log(so.name);
                    SceneManager.LoadScene(so.SceneName);
                }
            }
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.position += (_mainCam.transform.right * h + _mainCam.transform.forward * v).normalized * Time.deltaTime * _speed;
    }

    private void Rotate()
    {
        float x = -Input.GetAxis("Mouse Y") * Time.deltaTime * _rotateSpeed;
        float y = Input.GetAxis("Mouse X") * Time.deltaTime * _rotateSpeed;

        _yRotate = _mainCam.transform.eulerAngles.y + y;

        _xRotate = _xRotate + x;

        _xRotate = Mathf.Clamp(_xRotate, -90, 90);

        _mainCam.transform.eulerAngles = new Vector3(_xRotate, _yRotate, 0);
    }
}
