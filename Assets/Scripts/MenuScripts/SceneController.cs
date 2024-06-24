using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChange(string name)
    {
        ResetGameState();
        SceneManager.LoadScene(name);
        Time.timeScale = 1.0f;
    }

    private void ResetGameState()
    {
        // Reset game manager state
        if (GameManager.instance != null)
        {
            GameManager.instance.ResetGame();
        }

        // Reset player stats
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.ResetStats();
        }
    }
}
