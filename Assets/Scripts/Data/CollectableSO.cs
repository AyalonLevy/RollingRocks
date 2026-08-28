using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Collactable", menuName = "Rolling Rocks/Collactable")]
public class CollectableSO : ScriptableObject
{
    [Header ("Collectable Stats")]

    public string collectableName;
    [TextArea(3, 10)]
    public string collectableDescription;
    public Sprite sprite;
}
