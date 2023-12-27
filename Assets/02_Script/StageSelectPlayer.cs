using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPlayer : MonoBehaviour
{
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += new Vector3(h, 0, v).normalized;
    }
}
