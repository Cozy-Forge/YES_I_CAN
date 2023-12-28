using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject Can;
    // Start is called before the first frame update
    void Start()
    {
        Can = GameObject.Find("CAN");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Can.transform.position;
    }
}
