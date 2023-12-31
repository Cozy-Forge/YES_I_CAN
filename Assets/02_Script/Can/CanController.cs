using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TMP_Text JumpTMP;
    [SerializeField] private AudioClip sound;

    void Awake()
    {
        JumpTMP.enabled = false;
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        JumpTMP = GetComponentInChildren<TextMeshPro>();  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && JumpCool <= 0)
        {
            Vector3 forwardDirection = cam.transform.forward + Vector3.up;
            rb.AddForce(forwardDirection * ForcePower, ForceMode.VelocityChange);
            rb.angularVelocity += forwardDirection + UnityEngine.Random.insideUnitSphere * 10;
            SoundManager.Instance.SFXPlay("asdf", sound, transform);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10);
            JumpCool = 2;
            Instantiate(JumpEffect, EffectPos.position, Quaternion.identity);
        }else if(JumpCool > 0)
        {
            JumpTMP.enabled = true;
            JumpCool -= Time.deltaTime;
            JumpTMP.text = JumpCool.ToString("0");
            if(JumpCool <= 0)
            {
                JumpTMP.enabled = false;
            }
        }


    }

    private void OnAnimatorMove()
    {
        
    }

}
