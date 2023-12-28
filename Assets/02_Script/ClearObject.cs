using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        // 플레이어 비교
        //if(collision.GetType() == typeof(GameObject))
        //{
        //SceneManager.LoadScene("StageSelect");
        //}

        Debug.Log(1);
    }
}
