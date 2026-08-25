using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject settingsMenu;
    public GameObject endOfLevelMenu;
    public ParticleSystem endOfGameFlare;

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

        InitLevel();
    }

    private void InitLevel()
    {
        settingsMenu.SetActive(false);
        endOfLevelMenu.SetActive(false);
        LevelManager.Instance.InitializeLevel();
        Time.timeScale = 1;
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

    public void QuitGame()
    {
        Debug.Log("[GameManager] Exit Game");
    }

    public void MainMenu()
    {
        Debug.Log("[GameManager] Go to Main Menu");
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

        endOfLevelMenu.SetActive(true);
        endOfGameFlare.Play();

        // Play an outro message from the AI? -> this means not stopping the time (which is fine)
        _isPaused = true;
        //Time.timeScale = 0;
    }

    public void LoadNextLevel()
    {
        Debug.Log("[GameManager] Load next level");
    }
}
