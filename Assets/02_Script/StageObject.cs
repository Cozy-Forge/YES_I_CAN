using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public string SceneName => sceneName;

    [SerializeField] private float remainTime = 0;
    public float RemainTime
    {
        get { return remainTime; }
        set { remainTime = value; }
    }

    Transform outline;

    private void Start()
    {
        outline = transform.GetChild(0);
    }

    private void Update()
    {
        if (remainTime >= 0)
        {
            remainTime -= Time.deltaTime;
        }
        outline.gameObject.SetActive(remainTime >= 0);
    }
}
