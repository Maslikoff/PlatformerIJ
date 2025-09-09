using System;
using System.Collections;
using System.Collections.Generic;
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

    private void InitializeAttackTrigger()
    {
        if (_attackTrigger == null)
        {
            _attackTrigger = GetComponentInChildren<BoxCollider2D>();

            if (_attackTrigger == null)
                return;
        }

        _attackTrigger.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleTargetDetection(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        HandleTargetDetection(other);
    }

    private void HandleTargetDetection(Collider2D other)
    {
        if (_canAttack == false) 
            return;

        if (((1 << other.gameObject.layer) & _targetLayer) != 0)
        {
            Health targetHealth = other.GetComponent<Health>();

            if (targetHealth != null && targetHealth.IsAlive)
            {
                OnTargetDetected?.Invoke(other.gameObject);
                TryAttack(targetHealth);
            }
        }
    }

    private void TryAttack(Health targetHealth)
    {
        if (Time.time - _lastAttackTime < _attackCooldown) 
            return;

        Attack(targetHealth);
    }

    public void Attack(Health targetHealth)
    {
        if (targetHealth == null || targetHealth.IsAlive == false)
            return;

        targetHealth.TakeDamage(_attackDamage);
        _lastAttackTime = Time.time;

        OnAttack?.Invoke();
    }
}
