using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance { get; private set; }

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
}
