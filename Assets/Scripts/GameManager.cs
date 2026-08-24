using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject settingsMenu;
    private bool _isPaused = false;
    public bool IsPaused { get { return _isPaused; } }

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

        settingsMenu.SetActive(false);
    }

    public void SettingsMenu()
    {
        _isPaused = true;
        settingsMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        settingsMenu.SetActive(false);
        _isPaused = false;
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
    }

    public void LoadNextLevel()
    {
        Debug.Log("[GameManager] Load next level");
    }
}
