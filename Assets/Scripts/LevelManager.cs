using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Setup")]
    [SerializeField] private EndTileController[] tasks;

    [Header("Gepeto Settings")]
    [Tooltip("Make sure the string name corresponds to the folder name")]
    [SerializeField] private string levelName = "Level_01";
    [Tooltip("The time to complete the level - not really, just to show as if")]
    [SerializeField] private float timeInMinutes = 10.5f;
    [SerializeField] private GepetoAI gepetoAI;

    [Header("End Of Level Settings")]
    [SerializeField] private GameObject endOfLevelMenu;
    [SerializeField] private ParticleSystem endOfGameFlare;

    private bool[] _completedTasks;

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

        InitializeLevel();
    }

    public void InitializeLevel()
    {
        // Find all EndTileController in the scene
        tasks = FindObjectsByType<EndTileController>();
        _completedTasks = new bool[tasks.Length];

        if (tasks.Length == 0)
        {
            Debug.LogWarning("[LevelManageg] There are no tasks, please check the level again!");
            return;
        }

        SetTaskIDs();

        if (gepetoAI == null)
        {
            Debug.LogWarning("[LevelManageg] The chat is gone! Bring it back!");
            return;
        }

        gepetoAI.InitializeAI(levelName);

        if (endOfLevelMenu != null)
        {
            endOfLevelMenu.SetActive(false);
        }

        UIManager.Instance.InitializeUI(levelName, timeInMinutes * 60);
    }

    private void SetTaskIDs()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].TaskID = i;
            _completedTasks[i] = false;
        }
    }

    public void TaskComplete(int taskId)
    {
        _completedTasks[taskId] = true;

        CompleteLevel();
    }

    private void CompleteLevel()
    {
        bool allCompleted = true;

        foreach (var task in _completedTasks)
        {
            if (!task)
            {
                allCompleted = false;
            }
        }

        if (allCompleted)
        {
            Debug.Log("[LevelManageg] All tasks are completed, Level Finished!");

            endOfLevelMenu.SetActive(true);
            endOfGameFlare.Play();

            GameManager.Instance.EndLevel();
        }
        else
        {
            Debug.Log("[LevelManageg] Some tasks are not complete yet!");
        }
    }
}
