using UnityEngine;

public class EndTileController : HiddenController
{
    [Header("Target Settings")]
    [SerializeField] private GameObject target;
    [SerializeField] private float targetScaleFactor = 0.9f;

    private Rigidbody2D _targetRB;

    public int TaskID { get; set; }

    private void Awake()
    {
        if (target != null)
        {
            _targetRB = target.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning($"[EndTileController] {target.name} has no Rigidbody attached!");
            return;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the collider is the target we do the Action, else we ignore
        if (collision.gameObject == target)
        {
            TargetReached();
        }
    }

    private void TargetReached()
    {
        // Disable the animator because it is controlling the SpriteRenderer
        RevealTruth();

        target.transform.position = transform.position;
        target.transform.localScale = Vector3.one * targetScaleFactor;
        _targetRB.bodyType = RigidbodyType2D.Static;

        // Update the GameManager know that this task was performed
        LevelManager.Instance.TaskComplete(TaskID);
    }
}
