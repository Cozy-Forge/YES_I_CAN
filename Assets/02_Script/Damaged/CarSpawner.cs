using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private MoveObject _car;
    [SerializeField] private Vector3 _dir;
    [SerializeField] private float _carSpeed = 30f;
    [SerializeField] private float _carDeleteTime = 5f;
    [SerializeField] private float _carSpawnDelay = 2f;

    [SerializeField] private bool _frontIsRight = false;

    private void Start()
    {
        StartCoroutine(CarSpawnCo());
    }

    IEnumerator CarSpawnCo()
    {
        while(true)
        {
            MoveObject car = Instantiate(_car, transform.position, Quaternion.identity);
            if(!_frontIsRight)
                car.transform.forward = _dir.normalized;
            else
                car.transform.right = _dir.normalized;
            car.transform.parent = transform;

            car.Go(_dir, _carSpeed);
            Destroy(car, _carDeleteTime);

            yield return new WaitForSeconds(_carSpawnDelay);
        }
    }

}
