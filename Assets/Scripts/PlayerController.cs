using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float airSpeed;
    Vector2 moveInput;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            airSpeed = 6f;
                            return runSpeed;
                        }
                        else
                        {
                            airSpeed = 4f;
                            return walkSpeed;
                        }
                    }
                    else return airSpeed;
                }
                else
                {
                    airSpeed = 2f;
                    return 0;
                }
            }
            else return 0;
        }
    }


    [SerializeField] private bool _isMoving = false;
    public bool IsMoving 
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField] private bool _isRunning = false;
    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }


    [SerializeField] private bool _isFacingRight = true;
    public bool IsFacingRight 
    { 
        get => _isFacingRight; 
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        } 
    }

    private float jumpImpluse = 6f;

    private bool canDoubleJump = false;

    private bool CanMove
    {
        get => animator.GetBool(AnimationStrings.canMove);
    }

    private bool IsAlive
    {
        get => animator.GetBool(AnimationStrings.isAlive);
    }

    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;

    [SerializeField] List<Collider2D> attackHitBox = new List<Collider2D>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);

        }

    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        } else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        } else if (moveInput.x == 0) return;
    }

    public void OnRun(InputAction.CallbackContext context) 
    { 
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CanMove && IsAlive)
        {
            if (touchingDirections.IsGrounded)
            {
                canDoubleJump = true;
                animator.SetTrigger(AnimationStrings.jumpTrigger);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpluse);
            }

            else if (canDoubleJump && rb.linearVelocity.y > 0)
            {
                canDoubleJump = false;
                animator.SetTrigger(AnimationStrings.jumpTrigger);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpluse);
            }

            //Air Attack
            //animator.SetBool(AnimationStrings.hasAirAttack, false);
        }
    }

    public void OnAttack(InputAction.CallbackContext contect)
    {
        if (contect.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void EnableAttackHitBox(int index)
    {
        if (index >= 0 && index < attackHitBox.Count)
        {
            attackHitBox[index].enabled = true;
        }
        
    }

    public void DisableAttackHitBox(int index)
    {
        if (index >= 0 && index < attackHitBox.Count)
        {
            attackHitBox[index].enabled = false;
        }
    }
}
