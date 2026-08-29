using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    public Image iconImage;

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
    }
}
