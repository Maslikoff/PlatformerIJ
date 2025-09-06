using System;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ISpawnable
{
    [SerializeField] private int _value = 1;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private ParticleSystem _collectEffect;

    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    public event Action OnDespawn;

    public int Value => _value;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Collect();
    }

    public void Collect()
    {
        _collider.enabled = false;
        _spriteRenderer.enabled = false;

        if (_collectEffect != null)
            _collectEffect.Play();

        CoinSystem.Instance?.AddCoin(_value);

        Invoke(nameof(Despawn), 0.5f);
    }

    public void Despawn()
    {
        OnDespawn?.Invoke();
        Destroy(gameObject);
    }
}
