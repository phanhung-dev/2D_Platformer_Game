using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float airSpeed = 6f;
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
                        return moveSpeed;
                    }
                    else
                    {
                        return airSpeed;
                    }
                }
                else return 0;
            }
            else return 0;
        }
    }

    [Header("Slide")]
    [SerializeField] private float slideSpeed = 12f;      
    [SerializeField] private float slideDuration = 0.35f; 
    [SerializeField] private float slideCooldown = 2.0f;  
    private float nextSlideTime = 0f;
    private bool isSliding = false;
    public bool CanSlide => CanMove && IsAlive && touchingDirections.IsGrounded && !isSliding && Time.time >= nextSlideTime && !IsHit;

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

    [Header("Skill Projectile")]
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private Transform slashSpawnPoint;

    [Header("Skill UI")]
    [SerializeField] private Image cooldownSlash;
    [SerializeField] private float slashCoolDown = 5f;
    private float nextSlashTime = 0f;

    private bool _isHit = false;
    public bool IsHit
    {
        get => _isHit;
        private set
        {
            _isHit = value;
            animator.SetBool(AnimationStrings.isHit, value);
        }
    }

    private bool isPointerOverUI = false;

    [Header("Skills && Mana")]
    [SerializeField] private int slashManaCost = 20;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Update()
    {
        UpdateCooldownUI();

        if (EventSystem.current != null)
        {
            isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        }
    }

    private void FixedUpdate()
    {
        if (!IsHit && !isSliding)
        {
            rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
        }


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
        if (context.started && CanSlide)
        {
            StartCoroutine(PerformSlide());
        }
    }

    private IEnumerator PerformSlide()
    {
        isSliding = true;
        nextSlideTime = Time.time + slideCooldown;

        animator.SetTrigger(AnimationStrings.slideTrigger);

        float slideDirection = IsFacingRight ? 1f : -1f;

        float timer = 0f;
        while (timer < slideDuration)
        {
            rb.linearVelocity = new Vector2(slideDirection * slideSpeed, rb.linearVelocity.y);
            timer += Time.deltaTime;
            yield return null;
        }
        isSliding = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CanMove && IsAlive && !isSliding)
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

        }
    }

    public void OnAttack(InputAction.CallbackContext contect)
    {
        if (isPointerOverUI || isSliding || !IsAlive)
        {
            return;
        }

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

    public void OnSlash(InputAction.CallbackContext context)
    {
        if (context.started && !isSliding && CanMove)
        {
            if (Time.time >= nextSlashTime)
            {
                Mana playerMana = GetComponent <Mana>();
                if (playerMana != null && !playerMana.UseMana(slashManaCost))
                {
                    return;
                }


                animator.SetTrigger(AnimationStrings.skill01);
                nextSlashTime = Time.time + slashCoolDown;
            }
        }
    }

    public void FireSlash()
    {
        if (slashPrefab != null && slashSpawnPoint != null)
        {
            GameObject slash = Instantiate(slashPrefab, slashSpawnPoint.position, slashSpawnPoint.rotation);

            SlashProjectile projectileScript = slash.GetComponent<SlashProjectile>();
            if (projectileScript != null)
            {
                float facingDirection = IsFacingRight ? 1f : -1f;

                projectileScript.Initialize(facingDirection);
            }
        }
    }


    private void UpdateCooldownUI()
    {
        if (cooldownSlash != null)
        {
            if (Time.time < nextSlashTime)
            {
                float timeRemaining = nextSlashTime - Time.time;

                cooldownSlash.fillAmount = timeRemaining/slashCoolDown;
            }
            else
            {
                cooldownSlash.fillAmount = 0f;
            }
        }
    }

    public void OnHit()
    {
        StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        IsHit = true;

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        yield return new WaitForSeconds(0.4f);

        IsHit = false;
    }
}
