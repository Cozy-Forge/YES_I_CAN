using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Cinemachine;

public class StageSelectPlayer : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    private Rigidbody _rigid;
    private float _xRotate, _yRotate;

    [SerializeField] private GameObject _tooltip;

    [SerializeField] private float _rotateSpeed = 500.0f;
    [SerializeField] private float _speed = 5f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _cam.transform.rotation = Quaternion.identity;
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
        bool isHit = Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hitInfo, 3 /*_mainCam.farClipPlane*/, 1 << 10);

        _tooltip.SetActive(isHit);
        if (isHit)  
        {
            if (hitInfo.transform.TryGetComponent<StageObject>(out StageObject so))
            {
                so.RemainTime = 0.1f;
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

        //transform.position += (_mainCam.transform.right * h + _mainCam.transform.forward * v).normalized * Time.deltaTime * _speed;
        _rigid.velocity = ( _cam.transform.right * h + _cam.transform.forward * v);
        _rigid.velocity = new Vector3(_rigid.velocity.x, 0, _rigid.velocity.z).normalized * _speed;
    }

    private void Rotate()
    {
        float x = -Input.GetAxis("Mouse Y") * Time.deltaTime * _rotateSpeed;
        float y = Input.GetAxis("Mouse X") * Time.deltaTime * _rotateSpeed;

        _yRotate = _cam.transform.eulerAngles.y + y;

        _xRotate = _xRotate + x;

        _xRotate = Mathf.Clamp(_xRotate, -90, 90);

        _cam.transform.eulerAngles = new Vector3(_xRotate, _yRotate, 0);
    }
}
