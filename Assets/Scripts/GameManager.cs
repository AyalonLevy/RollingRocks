using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject settingsMenu;
    public GepetoAI gepetoAI;
    public Button continueButton;

    [HideInInspector] public GameData gameData;

    private int _currentLevel = 1;
    private int availableLevels = 1;
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
                _currentLevel = gameData.maxUnlockedLevel;
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

        if (gepetoAI != null && SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Only initialize the AI when it's in the main menu
            gepetoAI.InitializeAI("MainMenu");
        }

        // Handle levels
        availableLevels = SceneManager.sceneCountInBuildSettings;
        Debug.Log($"There are {availableLevels} available scenes");

        // Disable continue button if we never started the game - the maxlevel is less than 2
        if (gameData.maxUnlockedLevel < 2 && continueButton != null)
        {
            continueButton.interactable = false;
        }
    }

    private void Start()
    {
        // It is in the Start because the AudioMixer is not updated when it is in the Awake
        settingsMenu.GetComponent<SettingsMenu>().SetVolumeFromData(gameData.musicVolume, gameData.sfxVolume);
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
            Debug.Log($"Continue where we left off, on level {gameData.maxUnlockedLevel}");
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
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        _isPaused = false;
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
        _currentLevel = Mathf.Min(_currentLevel + 1, availableLevels);

        // Save new level to game Data
        if (_currentLevel > gameData.maxUnlockedLevel)
        {
            gameData.maxUnlockedLevel = _currentLevel;

            SaveSystem.SaveData(gameData);
        }

        Debug.Log($"Total scenes: {_totalScenes}");
        if (SceneManager.GetActiveScene().buildIndex == _totalScenes - 1)
        {
            Debug.Log("The last Scene!");
        }
        else
        {
            StartGame(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("[GameManager] Exit Game");
        Application.Quit();
    }

}
