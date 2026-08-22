using UnityEngine;
using UnityEngine.Events;
public class Mana : MonoBehaviour
{
    [SerializeField] private int _maxMana = 100;
    public int MaxMana => _maxMana;

    [SerializeField] private int _currentMana = 100;
    public int CurrentMana => _currentMana;

    public bool autoRegen = true;
    public float regenRate = 5f;
    private float regenTimer = 0f;

    public UnityEvent<int, int> manaChanged;

    private void Start()
    {
        _currentMana = _maxMana;
        manaChanged?.Invoke(_currentMana, _maxMana);
    }

    private void Update()
    {
        if (autoRegen && _currentMana < _maxMana)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= 1f)
            {
                RestoreMana((int)regenRate);
                regenTimer = 0f;
            }
        }
    }

    public bool UseMana(int amount)
    {
        if (_currentMana >= amount)
        {
            _currentMana -= amount;
            manaChanged?.Invoke(_currentMana, _maxMana);
            return true;
        }
        return false;
    }

    public void RestoreMana(int amount)
    {
        _currentMana += amount;
        if (_currentMana > _maxMana) _currentMana = _maxMana;
        manaChanged?.Invoke(_currentMana, _maxMana);
    }

    public void IncreaseMaxMana(int amount)
    {
        _maxMana += amount;
        _currentMana += amount;
        manaChanged?.Invoke(_currentMana, _maxMana);
    }
}
