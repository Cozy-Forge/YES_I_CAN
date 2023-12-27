using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Camera cam;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        LookAtMouse();
    }

    public void LookAtMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookPos);
        }
    }
}
