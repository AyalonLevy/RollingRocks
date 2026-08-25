using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject settingsMenu;

    private int _totalScenes;  // Last scene will be the End of Game Scene

    private bool _isPaused = false;
    public bool IsPaused { get { return _isPaused; } }

    private void Start()
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
        }
    }

    public void SettingsMenu()
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
