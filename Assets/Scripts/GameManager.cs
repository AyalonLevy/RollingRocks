using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject settingsMenu;

    [HideInInspector] public GameData gameData;

    private int _currentLevel = 0;
    private int _totalScenes;  // Last scene will be the End of Game Scene

    private bool _isPaused = false;
    public bool IsPaused { get { return _isPaused; } }

    private bool _useWASD = false;
    public bool UseWASD
    {
        get => _useWASD;
        set
        {
            if (_useWASD != value)
            {
                _useWASD = value;
                OnControlInputChange?.Invoke(_useWASD);

                // Save Settings
                gameData.useWASD = _useWASD;
                SaveSystem.SaveData(gameData);
            }
        }
    }

    public event Action<bool> OnControlInputChange;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _totalScenes = SceneManager.sceneCountInBuildSettings;

        // Load saved data and if there is none, create a new one
        gameData = SaveSystem.LoadData();
        gameData ??= new GameData();
        UseWASD = gameData.useWASD;
    }

    public GameData GetGameData()
    {
        return gameData;
    }

    private void InitLevel()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }

        Time.timeScale = 1;
    }

    public void StartGame(bool isNewGame)
    {
        if (isNewGame)
        {
            // Load the first level
            Debug.Log("Start from the beginning");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            // Load the most recent level
            Debug.Log("Continue where we left off");
            SceneManager.LoadScene(gameData.maxUnlockedLevel);
        }
    }

    public void ToggleGameState()
    {
        if (_isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _isPaused = true;
        settingsMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        settingsMenu.SetActive(false);
        _isPaused = false;
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        Debug.Log("[GameManager] Go to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        _isPaused = false;
        Debug.Log("[GameManager] Restarting level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // TODO: Do I need it here?
        InitLevel();
    }

    public void EndLevel()
    {
        HiddenManager.Instance.RevealAll();
        // ToDO:
        // 1. Pause game
        // 2. (?) Play an outro message from the AI -> this means not stopping the time (which is fine)
        // 3. (?) Start a victory dance

        _isPaused = true;
    }

    public void LoadNextLevel()
    {
        _currentLevel++;
        // Save new level to game Data
        if (_currentLevel > gameData.maxUnlockedLevel)
        {
            gameData.maxUnlockedLevel = _currentLevel;

            SaveSystem.SaveData(gameData);
        }

        Debug.Log("[GameManager] Load next level");
        Debug.Log($"Total scenes: {_totalScenes}");
        if (SceneManager.GetActiveScene().buildIndex == _totalScenes - 1)
        {
            Debug.Log("The last Scene!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("[GameManager] Exit Game");
        Application.Quit();
    }

}
