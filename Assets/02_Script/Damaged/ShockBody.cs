using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockBody : MonoBehaviour
{
    [SerializeField]
    private int _damage = 30;

    [SerializeField]
    private float _power = 20;

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.TryGetComponent<PlayerPollutionDegree>(out PlayerPollutionDegree pPollutionDegree))
        {
            pPollutionDegree.TakeDamage(_damage);
        }

        // ³Ë¹é
        if (col.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 dir = (col.transform.position - transform.position).normalized;

            rb.AddForce(dir * _power, ForceMode.Impulse);
        }
    }
}
