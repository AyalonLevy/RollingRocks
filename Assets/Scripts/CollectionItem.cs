using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text description;
    public TMP_Text descriptionShadow;
    public void SetCollectable(CollectableSO collectable)
    {
        iconImage.sprite = collectable.sprite;
        description.text = collectable.collectableDescription;
        descriptionShadow.text = collectable.collectableDescription;
    }
}
