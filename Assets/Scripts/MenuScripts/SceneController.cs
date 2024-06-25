using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
        GameManager.instance.ResetGame(); // Ensure the game manager resets the game state
        Time.timeScale = 1.0f;
    }
}
