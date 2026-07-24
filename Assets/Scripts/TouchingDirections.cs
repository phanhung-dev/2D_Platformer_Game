using System;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D touchingCol;
    Animator animator;

    [SerializeField] private ContactFilter2D castFilter;

    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private float groundDistance = 0.05f;
    [SerializeField] private bool _isGrounded = true;
    public bool IsGrounded 
    {
        get => _isGrounded;
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);

        }
    }

    private RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private float wallDistance = 0.2f;
    Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    [SerializeField] private bool _isOnWall = true;
    public bool IsOnWall
    {
        get => _isOnWall;
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);

        }
    }

    private RaycastHit2D[] cellingHits = new RaycastHit2D[5];
    private float cellingDistance = 0.05f;
    [SerializeField] private bool _isOnCelling = true;
    public bool IsOnCelling
    {
        get => _isOnCelling;
        private set
        {
            _isOnCelling= value;
            animator.SetBool(AnimationStrings.isOnCelling, value);
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCelling = touchingCol.Cast(Vector2.up, castFilter, cellingHits, cellingDistance) > 0;
    }
}
