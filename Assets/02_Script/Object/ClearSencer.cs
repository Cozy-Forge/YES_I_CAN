using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClearSencer : MonoBehaviour
{

    [SerializeField] private UnityEvent clearEvt;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            clearEvt?.Invoke();

        }

    }
}
