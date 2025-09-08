using System;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ISpawnable
{
    [SerializeField] private int _value = 1;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private ParticleSystem _collectEffect;

    public event Action<ISpawnable> Spawned;

    public int Value => _value;

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }

    public void PlayCollectEffect()
    {
        if (_collectEffect != null)
            _collectEffect.Play();
    }

    public void Despawn()
    {
        Spawned?.Invoke(this);
        Destroy(gameObject);
    }
}
