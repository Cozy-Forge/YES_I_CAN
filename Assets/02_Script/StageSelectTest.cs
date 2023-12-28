using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectTest : MonoBehaviour
{
    public void OnBtnDown()
    {
        SceneManager.LoadScene("StageSelect");
    }
}
