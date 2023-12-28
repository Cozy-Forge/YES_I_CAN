using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyObject : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private DirtyObjectInfoSO _info;
    [SerializeField] private Collider _collider;

    private void Awake()
    {
        if(_collider == null)
            _collider = GetComponent<Collider>();
        if (_collider == null)
            Debug.LogError("DirtyObject Collider is null");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_info == null) return;
        if (_collider == null) _collider = GetComponent<Collider>();

        gameObject.name = $"Dirty[ {_info.objectName} ]";
    }
#endif
    
    private void OnCollisionStay(Collision col)
    {
        if (_collider == null)
            return;
        if (_collider.isTrigger == true)
            return;

        Pollute(col.transform);
    }
    private void OnTriggerStay(Collider col)
    {
        if (_collider == null)
            return;
        if (_collider.isTrigger == false)
            return;

        Pollute(col.transform);
    }

    private void Pollute(Transform obj)
    {
        if(obj.TryGetComponent<PlayerPollutionDegree>(out PlayerPollutionDegree pPollutionDegree))
        {
            pPollutionDegree.TakeDamage(_info.dirtyValue * Time.deltaTime);
        }
    }
}
