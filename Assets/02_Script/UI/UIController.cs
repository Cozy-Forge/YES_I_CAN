using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public void TimeSet(float time)
    {

        Time.timeScale = time;

    }

    public void SceneChange(string sceneName)
    {

        SceneManager.LoadScene(sceneName);

    }

    private void OnDestroy()
    {

        Time.timeScale = 1;

    }

    public void Restart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
