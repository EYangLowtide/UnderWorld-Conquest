using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Use enum to define different states of the game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    // Store the current state of the game
    public GameState currentState;

    // Store the previous state of the game
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    // public GameObject levelUpScreen;

    [Header("Current Stat Display")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentRecoveryDisplay;
    public TMP_Text currentStrengthDisplay;
    public TMP_Text currentGutsDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentAttackSpeedDisplay;
    public TMP_Text currentDashRangeDisplay;
    public TMP_Text currentMagnetDisplay;

    [Header("Results Screen Display")]
    public Image chosenPlayerImage;
    public TMP_Text chosenPlayerName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(8);
    public List<Image> chosenPassiveUI = new List<Image>(8);

    [Header("StopWatch")]
    public float timeLimit;
    private float stopwatchTime;
    [SerializeField] public TMP_Text stopwatchDisplay;

    public bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist GameManager across scenes
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }

        DisableScreens();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopWatch();
                UpdatePlayerStatsUI();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("GAME OVER");
                    DisplayResults();
                }
                break;
            default:
                Debug.LogWarning("State Does Not EXIST");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; // Stops the game
            pauseScreen.SetActive(true);
            Debug.Log("GAME IS PAUSED");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // Resumes the game
            pauseScreen.SetActive(false);
            Debug.Log("Game Resume");
        }
    }

    private void CheckForPauseAndResume()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        // levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    private void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenPlayerUI(PlayerScriptableObject chosenPlayerData)
    {
        chosenPlayerImage.sprite = chosenPlayerData.Icon;
        chosenPlayerName.text = chosenPlayerData.Name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponAndPassiveUI(List<Image> chosenWeaponData, List<Image> chosenPassiveData)
    {
        if (chosenWeaponData.Count != chosenWeaponsUI.Count || chosenPassiveData.Count != chosenPassiveUI.Count)
        {
            Debug.Log("Chosen weapons and passive items have different lengths");
            return;
        }

        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveUI.Count; i++)
        {
            if (chosenPassiveData[i].sprite)
            {
                chosenPassiveUI[i].enabled = true;
                chosenPassiveUI[i].sprite = chosenPassiveData[i].sprite;
            }
            else
            {
                chosenPassiveUI[i].enabled = false;
            }
        }
    }

    private void UpdateStopWatch()
    {
        if (currentState == GameState.Gameplay)
        {
            stopwatchTime += Time.deltaTime;

            UpdateStopWatchDisplay();

            if (stopwatchTime >= timeLimit)
            {
                GameOver();
            }
        }
    }

    private void UpdateStopWatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        currentState = GameState.Gameplay;
        isGameOver = false;
        stopwatchTime = 0f; // Reset the stopwatch
        DisableScreens();
        // Optionally reset other game-related states and variables here
    }

    private void UpdatePlayerStatsUI()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            currentHealthDisplay.text = $"Health: {playerStats.CurrentHealth}";
            currentMoveSpeedDisplay.text = $"Move Speed: {playerStats.CurrentMoveSpeed}";
            currentRecoveryDisplay.text = $"Regeneration: {playerStats.CurrentRecovery}";
            currentStrengthDisplay.text = $"Strength: {playerStats.CurrentStrength}";
            currentGutsDisplay.text = $"Guts: {playerStats.CurrentGuts}";
            currentProjectileSpeedDisplay.text = $"Projectile Speed: {playerStats.CurrentProjectileSpeed}";
            currentAttackSpeedDisplay.text = $"Attack Speed: {playerStats.CurrentAttackSpeed}";
            currentDashRangeDisplay.text = $"Dash Range: {playerStats.CurrentDashRange}";
            currentMagnetDisplay.text = $"Magnet Range: {playerStats.CurrentMagnet}";
        }
    }
}
