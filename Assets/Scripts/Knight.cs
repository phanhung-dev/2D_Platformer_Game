using System;
using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(DectectionZone))]
public class Knight : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    [SerializeField] private DectectionZone attackZone;
    Animator animator;

    public enum WalkAbleDirection { Right, Left};
    private WalkAbleDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkAbleDirection WalkDirection
    {
        get => _walkDirection;
        private set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkAbleDirection.Right) { walkDirectionVector = Vector2.right; }
                else if (value == WalkAbleDirection.Left) { walkDirectionVector = Vector2.left; }
            }

            _walkDirection = value;
        }
    }

    [SerializeField] private float walkStopRate = 0.5f;

    private bool _hasTarget = false;
    public bool HasTarget 
    {
        get => _hasTarget; 
        private set
        {
           _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    private bool canMove
    {
        get => animator.GetBool(AnimationStrings.canMove);
    }

    private bool isTunning = false;
    private bool _facingDirection = false;

    public bool FacingDirection
    {
        get => _facingDirection;
        private set
        {
            _facingDirection = value;
        }
    }

    public bool IsIdle
    {
        get => animator.GetBool(AnimationStrings.isIdle);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && (touchingDirections.IsOnWall || FacingDirection) && !isTunning)
        {
            StartCoroutine(TurnAroundRountine());
        }

        if (canMove)
        {
            rb.linearVelocity = new Vector2(walkDirectionVector.x * walkSpeed, rb.linearVelocity.y);
            //Debug.Log("Dang di chuyen");

        }
        else
        { 
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
            //Debug.Log("Dang di cham lai");
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkAbleDirection.Right) { WalkDirection = WalkAbleDirection.Left; }
        else if (WalkDirection == WalkAbleDirection.Left) { WalkDirection = WalkAbleDirection.Right; }
        else Debug.LogError("Knight line 58: FlipDirection()");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PatrolBoundary"))
        {
            FacingDirection = true;
        }
    }

    private IEnumerator TurnAroundRountine()
    {
        isTunning = true;

        animator.SetBool(AnimationStrings.isIdle, true);

        yield return new WaitForSeconds(2f);

        FlipDirection();

        FacingDirection = false;

        animator.SetBool(AnimationStrings.isIdle, false);

        isTunning = false;
    }
}
