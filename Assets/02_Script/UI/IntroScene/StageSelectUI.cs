using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectUI : MonoBehaviour
{

    [SerializeField] private Transform buttonsParent;
    [SerializeField] private Transform panel;

    private void Awake()
    {
        
        for(int i = 0; i < buttonsParent.childCount; i++)
        {

            buttonsParent.GetChild(i).localPosition -= new Vector3(1100, 0, 0);

        }

        panel.transform.localPosition -= new Vector3(1100, 0, 0);

    }

}
