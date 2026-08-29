using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CollectionDatabaseSO", menuName = "Rolling Rocks/Collectable Database")]
public class CollectableDatabaseSO : ScriptableObject
{
    [SerializeField] private List<CollectableSO> allCollectables = new();

    private Dictionary<string, CollectableSO> lookupCache;

    public void Initialize()
    {
        lookupCache = new();

        foreach (CollectableSO collectable in allCollectables)
        {
            if (!lookupCache.ContainsKey(collectable.displayName))
            {
                lookupCache.Add(collectable.displayName, collectable);
            }
            else
            {
                Debug.LogError($"[CollectableDatabaseSO] Duplicate Collectable ID found: {collectable.displayName}");
            }
        }
    }

    public CollectableSO GetCollectableByID(string id)
    {
        if (lookupCache == null)
        {
            Initialize();
        }

        if (lookupCache.TryGetValue(id, out CollectableSO foundCollectable))
        {
            return foundCollectable;
        }

        return null;
    }
}
