using UnityEngine;

// TODO: All commented out sections are an attempt to use a class for the task, comeback to it later

//public class Task
//{
//    public int id;
//    public bool isComplete;

//    public void InitTask(int id)
//    {
//        this.id = id;
//        isComplete = false;
//    }

//    public void CompleteTask()
//    {
//        isComplete = true;
//    }
//}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Setup")]
    [SerializeField] private EndTileController[] tasks;

    //private Task[] _myTasks;
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

        // Find all EndTileController in the scene
        tasks = FindObjectsByType<EndTileController>();
        _completedTasks = new bool[tasks.Length];

        if (tasks.Length == 0)
        {
            Debug.LogWarning("There are no tasks, please check the level again!");
            return;
        }

        SetTaskIDs();
    }

    private void SetTaskIDs()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].SetTaskId(i);
            _completedTasks[i] = false;

            //_myTasks[i].InitTask(i);
        }
    }

    public void TaskComplete(int taskId)
    {
        _completedTasks[taskId] = true;

        //_myTasks[taskId].CompleteTask();
        CompleteLevel();
    }

    private void CompleteLevel()
    {
        bool allCompleted = true;
        //foreach (Task task in _myTasks)
        //{
        //    if (!task.isComplete)
        //        allCompleted = false;
        //}

        foreach (var task in _completedTasks)
        {
            if (!task)
            {
                allCompleted = false;
            }
        }

        if (allCompleted)
        {
            Debug.Log("All tasks are completed, Level Finished!");
        }
        else
        {
            Debug.Log("Some tasks are not complete yet!");
        }
    }
}
