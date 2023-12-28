using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private int _carDamage = 30;
    private float _power = 20;

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.TryGetComponent<PlayerPollutionDegree>(out PlayerPollutionDegree pPollutionDegree))
        {
            pPollutionDegree.TakeDamage(_carDamage);
        }

        // ³Ë¹é
        if (col.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 dir = (col.transform.position - transform.position).normalized;

            rb.AddForce(dir * 20, ForceMode.Impulse);
        }
    }
}
