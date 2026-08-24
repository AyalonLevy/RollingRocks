using System;
using UnityEngine;

public class EndTileController : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private GameObject target;
    [SerializeField] private Sprite realTile;
    [SerializeField] private float targetScaleFactor = 0.9f;

    [Header("Hint Animation Settings")]
    [SerializeField] private float animationInterval = 60.0f;
    [SerializeField] private float intervalShrinkingFactor = 5.0f;
    [SerializeField] private float minInterval = 5.0f;
    

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private float _intervalStartTime;
    private bool _playHintAnimation = true;
    private int _taskID;

    public void SetTaskId(int id) => _taskID = id;
    public int GetTaskId() => _taskID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _intervalStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playHintAnimation)
        {
            PlayAnimation();
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
        _animator.enabled = false;

        _spriteRenderer.sprite = realTile;
        _spriteRenderer.color = Color.white;

        _playHintAnimation = false;

        target.transform.position = transform.position;
        target.transform.localScale = Vector3.one * targetScaleFactor;
        target.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Update the GameManager know that this task was performed
        LevelManager.Instance.TaskComplete(_taskID);
    }

    private void PlayAnimation()
    {
        if (_animator == null) return;

        if (Time.time - _intervalStartTime > animationInterval)
        {
            _animator.Play("Hint", -1, 0.0f);
            _intervalStartTime = Time.time;
            animationInterval = animationInterval > minInterval ? animationInterval - intervalShrinkingFactor : animationInterval;
        }
    }
}
