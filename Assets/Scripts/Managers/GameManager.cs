using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject settingsMenu;
    public GameObject levelSelectPanel;
    public GameObject CollectionPanel;
    public GepetoAI gepetoAI;
    public Button continueButton;
    public Animator transition;
    public float transitionTime = 1.0f;


    [HideInInspector] public GameData gameData;

    private int _currentLevel = 1;
    private int _totalScenes;  // First scene is Main Menu and last scene will be the End of Game Scene
    private int _availableLevels;

    public int AvailableLevels { get { return _availableLevels; } }

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
        _availableLevels = _totalScenes - 2;

        // Load saved data and if there is none, create a new one
        gameData = SaveSystem.LoadData();
        gameData ??= new GameData();
        UseWASD = gameData.useWASD;
        _currentLevel = gameData.maxUnlockedLevel;

        if (gepetoAI != null && SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Only initialize the AI when it's in the main menu
            gepetoAI.InitializeAI("MainMenu");
        }

        // Disable continue button if we never started the game - the maxlevel is less than 2
        if (gameData.maxUnlockedLevel < 2 && continueButton != null)
        {
            continueButton.interactable = false;
        }
    }

    private void Start()
    {
        // It is in the Start because the AudioMixer is not updated when it is in the Awake
        if (settingsMenu != null)
        {
            settingsMenu.GetComponent<SettingsMenu>().SetVolumeFromData(gameData.musicVolume, gameData.sfxVolume);
        }
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
            StartCoroutine(LoadLevel(1));
        }
        else
        {
            // Load the most recent level
            _currentLevel = Mathf.Min(_totalScenes - 1, _currentLevel);
            Debug.Log($"Starting from level: {_currentLevel}");
            StartCoroutine(LoadLevel(_currentLevel));
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
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(0));
    }

    public void RestartLevel()
    {
        _isPaused = false;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));

        // TODO: Do I need it here?
        InitLevel();
    }

    public void EndLevel()
    {
        HiddenManager.Instance.RevealAll();
        // ToDO:
        gepetoAI.DisplayOutroText();

        _currentLevel = Math.Min(SceneManager.GetActiveScene().buildIndex + 1, _totalScenes);

        // Save new level to Game Data -> even if not playing the next level, it is unlocked
        if (_currentLevel > gameData.maxUnlockedLevel)
        {
            gameData.maxUnlockedLevel = _currentLevel;

            SaveSystem.SaveData(gameData);
        }

        _isPaused = true;
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(_currentLevel));
    }

    public void LoadCustomLevel(int level)
    {
        Debug.Log($"Loading level {level}");
        StartCoroutine(LoadLevel(level));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void OpenLevelSelector()
    {
        levelSelectPanel.SetActive(true);
    }

    public void CloseLevelSelector()
    {
        levelSelectPanel.SetActive(false);
    }

    public void OpenCollection()
    {
        CollectionPanel.SetActive(true);
        CollectionManager.Instance.DisplayCollection();
    }

    public void CloseCollection()
    {
        CollectionPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("[GameManager] Exit Game");
        Application.Quit();
    }
}
