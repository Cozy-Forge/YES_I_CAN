using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        // �÷��̾� ��
        //if(collision.GetType() == typeof(GameObject))
        //{
        //SceneManager.LoadScene("StageSelect");
        //}

        Debug.Log(1);
    }
}
