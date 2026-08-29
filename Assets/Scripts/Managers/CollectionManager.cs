using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance { get; private set; }

    [Header("Display Collection Settings")]
    [SerializeField] private CollectableDatabaseSO collectionDB;
    [SerializeField] private GameObject collectionItemPrefab;
    [SerializeField] private Transform grid;

    [HideInInspector]
    public List<CollectableSO> playerCollection = new();

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

    public void Collect(CollectableSO collectedItem)
    {
        if (playerCollection.Contains(collectedItem))
        {
            return;
        }

        playerCollection.Add(collectedItem);

        SaveCollection();
    }

    public void SaveCollection()
    {
        GameData gameData = GameManager.Instance.GetGameData();

        foreach (CollectableSO item in playerCollection)
        {
            if (!gameData.collectedItemIDs.Contains(item.displayName))
            {
                gameData.collectedItemIDs.Add(item.displayName);

            }
        }

        Debug.Log($"Current Collection has {gameData.collectedItemIDs.Count} items");
        SaveSystem.SaveData(gameData);
    }

    public void LoadCollection()
    {
        GameData gameData = GameManager.Instance.GetGameData();

        foreach (string id in gameData.collectedItemIDs)
        {
            CollectableSO collectable = collectionDB.GetCollectableByID(id);

            if (collectable != null)
            {
                playerCollection.Add(collectable);
            }
        }
    }

    public void DisplayCollection()
    {
        LoadCollection();

        // Clean up previous populated grid
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }

        // Instantiate each collectable item
        for (int i = 0; i < playerCollection.Count; i++)
        {
            GameObject go = Instantiate(collectionItemPrefab, grid);
            go.name = playerCollection[i].displayName;

            if (go.TryGetComponent<CollectionItem>(out var ci))
            {
                ci.SetCollectable(playerCollection[i]);
            }
        }
    }
}
