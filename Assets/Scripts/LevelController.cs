using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public void StartGame()
    {
        Invoke("StartScene", 16f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartScene();
        }
    }
    private void StartScene()
    {
        SceneManager.LoadScene("Alfa");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
