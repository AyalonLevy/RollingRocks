using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TMP_Text levelName;
    [SerializeField] private TMP_Text timer;

    [Header("Time settings")]
    [SerializeField] private int minTimeStep = 1;
    [SerializeField] private int maxTimeStep = 3;

    private float _timeLeft;
    private float _timeUpdateInterval = 1;
    private float _lastUpdate = 0;

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
    }

    private void Update()
    {
        if (Time.time - _lastUpdate > _timeUpdateInterval)
        {
            _lastUpdate = Time.time;
            _timeLeft -= Random.Range(minTimeStep, maxTimeStep);

            UpdateTime();
        }
    }

    public void InitializeUI(string name, float time)
    {
        _timeLeft = time;
        levelName.text = name.Replace("_", " ");
        UpdateTime();
    }

    private void UpdateTime()
    {
        _lastUpdate = Time.time;
        timer.text = TimeSpan.FromSeconds(_timeLeft).ToString("mm\\:ss");
    }

    public string GetTime()
    {
        return timer.text;
    }
}
