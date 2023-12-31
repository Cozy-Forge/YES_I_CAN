using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingDie : MonoBehaviour
{

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private UnityEvent dieEvt;
    [SerializeField] private GameObject can;

    private float t;

    private void Update()
    {
        
        if(rigid.velocity.y < -0.5f)
        {

            t += Time.deltaTime;

        }
        else
        {

            t = 0;

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(t > 1f)
        {

            can.transform.localScale = new Vector3(0.2f, 1, 1);

            dieEvt?.Invoke();

        }

    }

}
