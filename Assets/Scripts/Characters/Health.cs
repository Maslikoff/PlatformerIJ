using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;

    public event Action<int> HealthChanged;
    public event Action<int> DamageTaken;
    public event Action<int> Healed;
    public event Action Death;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public bool IsAlive => _currentHealth > 0;
    public float HealthPercentage => (float)_currentHealth / _maxHealth;

    private void Start()
    {
        Initialize();
    }

    public void Initialize(int maxHealth = -1)
    {
        if (maxHealth > 0)
            _maxHealth = maxHealth;

        _currentHealth = _maxHealth;
        HealthChanged?.Invoke(_currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (IsAlive == false || damage <= 0) 
            return;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        HealthChanged?.Invoke(_currentHealth);
        DamageTaken?.Invoke(damage);

        if (_currentHealth <= 0)
            Die();
    }

    public void Heal(int healAmount)
    {
        if (IsAlive == false || healAmount <= 0) 
            return;

        int actualHeal = Mathf.Min(_maxHealth - _currentHealth, healAmount);
        _currentHealth += actualHeal;

        HealthChanged?.Invoke(_currentHealth);
        Healed?.Invoke(actualHeal);
    }

    public void HealFull()
    {
        Heal(_maxHealth);
    }

    public void SetHealth(int health)
    {
        _currentHealth = Mathf.Clamp(health, 0, _maxHealth);
        HealthChanged?.Invoke(_currentHealth);
    }

    public void IncreaseMaxHealth(int amount, bool healToFull = false)
    {
        _maxHealth += amount;

        if (healToFull)
            _currentHealth = _maxHealth;

        HealthChanged?.Invoke(_currentHealth);
    }

    private void Die()
    {
        Death?.Invoke();
    }
}
