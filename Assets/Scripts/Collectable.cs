using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSO collectable;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null) return;

        _spriteRenderer.sprite = collectable.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{collectable.collectableName} Collectable: {collision.name}");
        CollectionManager.Instance.Collect(collectable);

        Destroy(gameObject);
    }
}
