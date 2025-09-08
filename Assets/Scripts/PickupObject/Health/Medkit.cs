using System;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour, ISpawnable
{
    [SerializeField] private int _healAmount = 25;
    [SerializeField] private ParticleSystem _healEffect;

    public event Action<ISpawnable> Spawned;

    public int HealAmount => _healAmount;

    public void PlayHealthEffect()
    {
        if(_healEffect != null)
            _healEffect.Play();
    }

    public void Despawn()
    {
        Spawned?.Invoke(this);
        Destroy(gameObject);
    }
}
