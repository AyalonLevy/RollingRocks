using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;

    private Vector2 movement;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            animator.SetFloat("LastHorizontal", movement.x);
            animator.SetFloat("LastVertical", movement.y);
        }

        movement = context.ReadValue<Vector2>();
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.collider.CompareTag("Rock"))
    //    {
    //        Rigidbody2D rbody = collision.collider.GetComponent<Rigidbody2D>();

    //        if (rbody != null)
    //        {
    //            Vector2 step = GetMoveDirection();

    //            rbody.MovePosition(new(transform.position.x + step.x, transform.position.y + step.y));
    //        }
    //    }
    //}

    //private Vector2 GetMoveDirection()
    //{
    //    return new(Math.Sign(movement.x), Math.Sign(movement.y));
    //}
}
