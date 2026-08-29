using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform grid;

    private int _levelButtonsToSpawn;

    private void Start()
    {
        _levelButtonsToSpawn = GameManager.Instance.AvailableLevels;

        PopulateGrid();
    }

    public void PopulateGrid()
    {
        // TODO: Is it necessary? 
        // Clean up previous populated grid
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }

        GameData gameData = GameManager.Instance.GetGameData();

        // Instantiate each button
        for (int i = 0; i < _levelButtonsToSpawn; i++)
        {
            GameObject go = Instantiate(levelButtonPrefab, grid);
            go.name = $"Level_{i + 1}";

            TMP_Text[] textComponents = go.GetComponentsInChildren<TMP_Text>();

            // Set the text
            foreach (TMP_Text textComponent in textComponents)
            {
                if (textComponent != null)
                {
                    textComponent.text = (i + 1).ToString();
                }
            }

            // Set the button functionality
            int buttonIdx = i + 1;
            if (go.TryGetComponent<Button>(out var levelButton))
            {
                levelButton.onClick.AddListener(() => GameManager.Instance.LoadCustomLevel(buttonIdx));
            }

            // Set button to inactive if level wasan't unlocked yet
            if (buttonIdx > gameData.maxUnlockedLevel)
            {
                levelButton.interactable = false;
            }

        }
    }
}
