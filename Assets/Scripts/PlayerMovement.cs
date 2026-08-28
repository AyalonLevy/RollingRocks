using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;

    private Vector2 _movement;

    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerInput _playerInput;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();

        SetControls(GameManager.Instance.UseWASD);
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnControlInputChange += SetControls;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnControlInputChange -= SetControls;
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + moveSpeed * Time.fixedDeltaTime * _movement);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            AudioManager.Instance.Play("ScrapingRock");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            AudioManager.Instance.Stop("ScrapingRock");
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsPaused) return;

        if (context.canceled)
        {
            _animator.SetFloat("LastHorizontal", _movement.x);
            _animator.SetFloat("LastVertical", _movement.y);
        }

        _movement = context.ReadValue<Vector2>();

        _animator.SetFloat("Horizontal", _movement.x);
        _animator.SetFloat("Vertical", _movement.y);
        _animator.SetFloat("Speed", _movement.sqrMagnitude);
    }

    public void SetControls(bool value)
    {
        if (value)
        {
            _playerInput.SwitchCurrentActionMap("WASD");
        }
        else
        {
            _playerInput.SwitchCurrentActionMap("UDLR");
        }
    }
}
