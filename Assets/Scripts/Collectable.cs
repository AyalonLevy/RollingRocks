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

        _spriteRenderer.sprite = collectable.icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectionManager.Instance.Collect(collectable);
            AudioManager.Instance.Play("CollectItem");

            Destroy(gameObject);
        }
    }
}
