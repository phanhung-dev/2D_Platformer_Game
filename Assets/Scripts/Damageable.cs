using UnityEngine;

public class Damageable : MonoBehaviour
{
    Animator animator;

    [SerializeField] private int _maxHealth = 100;
    public int MaxHealth
    {
        get => _maxHealth;
        private set
        {
            _maxHealth = value;
        }
    }

    [SerializeField] private int _health = 100;
    public int Health
    {
        get => _health;
        private set
        {
            _health = value;

            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField] private bool _isAlive = true;

    public bool IsAlive 
    {
        get => _isAlive;
        private set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
        }
    }

    [SerializeField] private bool isInvincible = false;
    private float timeSinceHit = 0f;
    private float invicibilityTime = 0.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invicibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    public void Hit(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
        }
    }
}
