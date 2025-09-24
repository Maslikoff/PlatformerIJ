using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireAbility : MonoBehaviour
{
    [Header("Vampire Settings")]
    [SerializeField] private float _abilityDuration = 6f;
    [SerializeField] private float _cooldownDuration = 4f;
    [SerializeField] private float _damagePlayerSecond = 10f;
    [SerializeField] private float _healPlayerSecond = 10f;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private LayerMask _targetLayer;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer _radiusIndicator;
    [SerializeField] private Vector2 _sizeSprite = new Vector2(1.2f, 1.2f);
    [SerializeField] private Color _activeColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private Color _cooldownColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);

    private bool _isAbilityActive = false;
    private bool _isOnCooldown = false;
    private float _abilityTimer = 0f;
    private float _cooldownTimer = 0f;

    private Health _playerHealth;
    private List<Health> _enemiesInRange = new List<Health>();

    public event Action<float> AbilityProgressChanged;
    public event Action<float> CooldownProgressChanged;
    public event Action<bool> AbilityStateChanged;

    public bool IsAbilityActive => _isAbilityActive;
    public bool IsOnCooldown => _isOnCooldown;
    public float AbilityProgress => _abilityTimer / _abilityDuration;
    public float CooldownProgress => 1f - (_cooldownTimer / _cooldownDuration);

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
        InitializeRadiusIndicator();
    }

    private void Update()
    {
        UpdateAbility();
        UpdateCooldown();
        UpdateRadiusIndicator();
    }

    public void TryActivateAbility()
    {
        if (_isAbilityActive || _isOnCooldown)
            return;

        StartAbility();
    }

    private void InitializeRadiusIndicator()
    {
        _radiusIndicator.transform.localScale = _sizeSprite;
        _radiusIndicator.color = _cooldownColor;
        _radiusIndicator.gameObject.SetActive(false);
    }

    private void StartAbility()
    {
        _isAbilityActive = true;
        _abilityTimer = _abilityDuration;
        AbilityStateChanged?.Invoke(this);
        StartCoroutine(AbilityRoutine());
    }

    private IEnumerator AbilityRoutine()
    {
        float tickRate = 0.1f;
        float tickTimer = 0f;

        while (_abilityTimer > 0f)
        {
            _abilityTimer -= Time.deltaTime;
            AbilityProgressChanged?.Invoke(AbilityProgress);

            tickTimer += Time.deltaTime;

            if (tickTimer >= tickRate)
            {
                ProcessVampirism(tickRate);
                tickTimer = 0f;
            }

            yield return null;
        }

        EndAbility();
    }

    private void ProcessVampirism(float timeInterval)
    {
        FindEnemiesInRange();

        if (_enemiesInRange.Count == 0)
            return;

        Health closestEnemy = GetClosestEnemy();

        if (closestEnemy != null && closestEnemy.IsAlive)
        {
            float damage = _damagePlayerSecond * timeInterval;
            closestEnemy.TakeDamage(Mathf.RoundToInt(damage));

            float healAmount = _healPlayerSecond * timeInterval;
            _playerHealth.Heal(healAmount);
        }
    }

    private void FindEnemiesInRange()
    {
        _enemiesInRange.Clear();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius, _targetLayer);

        foreach (Collider2D hit in hits)
            if (hit.TryGetComponent(out Health health) && health != _playerHealth && health.IsAlive)
                _enemiesInRange.Add(health);
    }

    private Health GetClosestEnemy()
    {
        Health closest = null;
        float closestDistance = float.MaxValue;

        foreach (var enemy in _enemiesInRange)
        {
            if (enemy == null)
                continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    private void UpdateRadiusIndicator()
    {
        if (_radiusIndicator == null)
            return;

        if (_isAbilityActive)
        {
            _radiusIndicator.gameObject.SetActive(true);
            _radiusIndicator.color = _activeColor;
        }
        else if (_isOnCooldown)
        {
            _radiusIndicator.gameObject.SetActive(true);
            _radiusIndicator.color = _cooldownColor;
        }
        else
        {
            _radiusIndicator.gameObject.SetActive(false);
        }
    }

    private void UpdateCooldown()
    {
        if (_isOnCooldown)
        {
            _cooldownTimer -= Time.deltaTime;
            CooldownProgressChanged?.Invoke(CooldownProgress);

            if (_cooldownTimer <= 0f)
            {
                _isOnCooldown = false;
                CooldownProgressChanged?.Invoke(1f);
                AbilityStateChanged?.Invoke(false);
            }
        }
    }

    private void UpdateAbility()
    {
        if (_isAbilityActive)
        {
            _abilityTimer -= Time.deltaTime;
            AbilityProgressChanged?.Invoke(AbilityProgress);

            if (_abilityTimer <= 0f)
                EndAbility();
        }
    }

    private void EndAbility()
    {
        _isAbilityActive = false;
        _isOnCooldown = true;
        _cooldownTimer = _cooldownDuration;
        AbilityStateChanged?.Invoke(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
