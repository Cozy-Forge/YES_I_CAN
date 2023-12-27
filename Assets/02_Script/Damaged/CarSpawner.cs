using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _car;
    [SerializeField] private Vector3 _dir;
    [SerializeField] private float _carSpeed = 30f;
    [SerializeField] private float _carDeleteTime = 5f;
    [SerializeField] private float _carSpawnDelay = 2f;

    private void Start()
    {
        StartCoroutine(CarSpawnCo());
    }

    IEnumerator CarSpawnCo()
    {
        while(true)
        {
            GameObject car = Instantiate(_car, transform.position, Quaternion.identity);
            car.transform.forward = _dir.normalized;
            car.transform.parent = transform;

            car.GetComponent<Rigidbody>().velocity = _dir * _carSpeed;
            Destroy(car, _carDeleteTime);

            yield return new WaitForSeconds(_carSpawnDelay);
        }
    }

}
