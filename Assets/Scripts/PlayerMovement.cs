using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;

    private Vector2 _movement;

    private Rigidbody2D _rb;
    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + moveSpeed * Time.fixedDeltaTime * _movement);
    }

    public void Move(InputAction.CallbackContext context)
    {
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
}
