using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Alfa");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
