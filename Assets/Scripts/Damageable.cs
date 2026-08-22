using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent damageHit;

    public UnityEvent<int, int> healthChanged;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        healthChanged?.Invoke(Health, MaxHealth);
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

    public void Hit(int damage) //Tru mau khi bi danh
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            damageHit?.Invoke();
        }

        healthChanged?.Invoke(Health, MaxHealth);
    }

    public void Hit(int damage, Vector2 knockBack) //Hat tung ra dang sau
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            damageHit?.Invoke();

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = knockBack;
            }
        }

        healthChanged?.Invoke(Health, MaxHealth);
    }

    public void Heal(int healRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            Health += healRestore;

            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        healthChanged?.Invoke(Health, MaxHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount;
        Health += amount; 

        healthChanged?.Invoke(Health, MaxHealth);
    }
}
