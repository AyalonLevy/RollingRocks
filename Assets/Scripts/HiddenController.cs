using UnityEngine;

public class HiddenController : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private Sprite fakeTile;
    [SerializeField] private Sprite realTile;

    [Header("Hint Animation Settings")]
    [SerializeField] private bool playHintAnimation = true;
    [SerializeField] private Animator animator;
    [SerializeField] private float animationInterval = 60.0f;
    [SerializeField] private float intervalShrinkingFactor = 5.0f;
    [SerializeField] private float minInterval = 5.0f;

    private SpriteRenderer _spriteRenderer;
    private float _intervalStartTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (animator == null)
        {
            // In case it is not set in the Inspector take from the child
            animator = GetComponentInChildren<Animator>();
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _intervalStartTime = Time.time;

        HideTruth();
    }

    // Update is called once per frame
    void Update()
    {
        if (playHintAnimation)
        {
            PlayAnimation();
        }
    }

    private void PlayAnimation()
    {
        if (animator == null)
        {
            Debug.Log("[HiddenController] No animator found");
            return;
        }

        if (Time.time - _intervalStartTime > animationInterval)
        {
            animator.Play("Hint", -1, 0.0f);
            _intervalStartTime = Time.time;
            animationInterval = animationInterval > minInterval ? animationInterval - intervalShrinkingFactor : animationInterval;
        }
    }

    public void HideTruth()
    {
        _spriteRenderer.sprite = fakeTile;

        playHintAnimation = true;

        animator.gameObject.SetActive(true);
    }

    public void RevealTruth()
    {
        playHintAnimation = false;

        animator.gameObject.SetActive(false);

        _spriteRenderer.sprite = realTile;
    }
}
