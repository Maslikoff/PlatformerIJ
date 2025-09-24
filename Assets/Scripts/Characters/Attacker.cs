using System;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private LayerMask _targetLayer;

    [Header("Attack Zone")]
    [SerializeField] private BoxCollider2D _attackTrigger;

    private float _lastAttackTime;
    private bool _canAttack = true;

    public event Action OnAttack;
    public event Action<GameObject> OnTargetDetected;

    public int AttackDamage => _attackDamage;
    public bool CanAttack => _canAttack;

    private void Awake()
    {
        InitializeAttackTrigger();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleTargetDetection(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        HandleTargetDetection(other);
    }

    public void Attack(Health targetHealth)
    {
        if (targetHealth == null || targetHealth.IsAlive == false)
            return;

        targetHealth.TakeDamage(_attackDamage);
        _lastAttackTime = Time.time;

        OnAttack?.Invoke();
    }

    private void InitializeAttackTrigger()
    {
        if (_attackTrigger == null)
        {
            Debug.LogError($"AttackTrigger is not set in {gameObject.name}! Please assign it in the inspector.", this);

            enabled = false;
            throw new NullReferenceException("AttackTrigger reference is missing!");
        }

        if (_attackTrigger.isTrigger == false)
        {
            Debug.LogWarning($"AttackTrigger on {gameObject.name} should be set as trigger. Fixing automatically.", this);

            _attackTrigger.isTrigger = true;
        }
    }

    private void HandleTargetDetection(Collider2D other)
    {
        if (_canAttack == false)
            return;

        if (((1 << other.gameObject.layer) & _targetLayer) != 0)
        {
            if (other.TryGetComponent(out Health targetHealth))
            {
                if (targetHealth.IsAlive)
                {
                    OnTargetDetected?.Invoke(other.gameObject);
                    TryAttack(targetHealth);
                }
            }
        }
    }

    private void TryAttack(Health targetHealth)
    {
        if (Time.time - _lastAttackTime < _attackCooldown)
            return;

        Attack(targetHealth);
    }
}