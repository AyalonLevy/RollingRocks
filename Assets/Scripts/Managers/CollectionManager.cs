using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance { get; private set; }

    [Header("Display Collection Settings")]
    [SerializeField] private GameObject collectionItemPrefab;
    [SerializeField] private Transform grid;

    [HideInInspector]
    public List<CollectableSO> playerCollection;

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

        playerCollection.Clear();
    }

    public void Collect(CollectableSO collectedItem)
    {
        Debug.Log($"Collected {collectedItem.collectableName}");
        if (playerCollection.Contains(collectedItem))
        {
            Debug.Log($"Already collected {collectedItem.collectableName}!");
            return;
        }

        playerCollection.Add(collectedItem);

        SaveCollection(playerCollection);
    }

    public void SaveCollection(List<CollectableSO> collection)
    {
        GameData gameData = GameManager.Instance.GetGameData();

        gameData.playerCollectabels = collection;

        SaveSystem.SaveData(gameData);
    }

    public void LoadCollection(List<CollectableSO> collection)
    {
        if (collection == null)
        {
            playerCollection = new List<CollectableSO>();
        }
        else
        {
            playerCollection.AddRange(collection);
        }
    }

    public void DisplayCollection()
    {
        LoadCollection(GameManager.Instance.GetGameData().playerCollectabels);

        Debug.Log($"I have collected {playerCollection.Count} items, I'm going to display them under {grid.name}");

        // Clean up previous populated grid
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }

        // Instantiate each collectable item
        for (int i = 0; i < playerCollection.Count; i++)
        {
            GameObject go = Instantiate(collectionItemPrefab, grid);
            go.name = playerCollection[i].collectableName;

            if (go.TryGetComponent<CollectionItem>(out var ci))
            {
                ci.SetIcon(playerCollection[i].sprite);
            }
            

            //TMP_Text[] textComponents = go.GetComponentsInChildren<TMP_Text>();

            //// Set the text
            //foreach (TMP_Text textComponent in textComponents)
            //{
            //    if (textComponent != null)
            //    {
            //        textComponent.text = (i + 1).ToString();
            //    }
            //}

            //// Set the button functionality
            //int buttonIdx = i + 1;
            //if (go.TryGetComponent<Button>(out var levelButton))
            //{
            //    levelButton.onClick.AddListener(() => GameManager.Instance.LoadCustomLevel(buttonIdx));
            //}

            //// Set button to inactive if level wasan't unlocked yet
            //if (buttonIdx > gameData.maxUnlockedLevel)
            //{
            //    levelButton.interactable = false;
            //}

        }
    }
}
