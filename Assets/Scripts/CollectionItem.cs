using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text displayName;
    public TMP_Text displayNameShadow;
    public TMP_Text description;
    public TMP_Text descriptionShadow;
    public void SetCollectable(CollectableSO collectable)
    {
        iconImage.sprite = collectable.icon;
        displayName.text = collectable.displayName;
        displayNameShadow.text = collectable.displayName;
        description.text = collectable.itemDescription;
        descriptionShadow.text = collectable.itemDescription;
    }
}
