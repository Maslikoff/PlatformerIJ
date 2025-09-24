using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxValue = 100f;
    [SerializeField] private float _currentValue;

    public event Action<float> Changed;
    public event Action<float> DamageTaken;
    public event Action<float> Healed;
    public event Action Died;

    public float CurrentValue => _currentValue;
    public float MaxValue => _maxValue;
    public bool IsAlive => _currentValue > 0;
    public float Percentage => _currentValue / _maxValue;

    private void Start()
    {
        Initialize();
    }

    public void Initialize(int maxHealth = -1)
    {
        if (maxHealth > 0)
            _maxValue = maxHealth;

        _currentValue = _maxValue;
        Changed?.Invoke(_currentValue);
    }

    public void TakeDamage(int damage)
    {
        if (IsAlive == false || damage <= 0) 
            return;

        _currentValue = Mathf.Max(0, _currentValue - damage);
        Changed?.Invoke(_currentValue);
        DamageTaken?.Invoke(damage);

        if (_currentValue <= 0)
            Die();
    }

    public void Heal(float healAmount)
    {
        if (IsAlive == false || healAmount <= 0) 
            return;

        float actualHeal = Mathf.Min(_maxValue - _currentValue, healAmount);
        _currentValue += actualHeal;

        Changed?.Invoke(_currentValue);
        Healed?.Invoke(actualHeal);
    }

    public void HealFull()
    {
        Heal(_maxValue);
    }

    public void SetValue(int health)
    {
        _currentValue = Mathf.Clamp(health, 0, _maxValue);
        Changed?.Invoke(_currentValue);
    }

    public void IncreaseMaxHealth(int amount, bool healToFull = false)
    {
        _maxValue += amount;

        if (healToFull)
            _currentValue = _maxValue;

        Changed?.Invoke(_currentValue);
    }

    private void Die()
    {
        Died?.Invoke();

        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite == null) yield break;

        float fadeDuration = 1f;
        float timer = 0f;
        Color originalColor = sprite.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        Destroy(gameObject);
    }
}
