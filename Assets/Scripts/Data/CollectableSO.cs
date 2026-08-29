using UnityEngine;

[CreateAssetMenu(fileName = "New Collactable", menuName = "Rolling Rocks/Collactable")]
public class CollectableSO : ScriptableObject
{
    [Header("Collectable Stats")]
    public string displayName;
    [TextArea(3, 10)]
    public string itemDescription;
    public Sprite icon;
}
