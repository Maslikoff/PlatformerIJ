using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private CoinSystem _coinSystem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Coin>(out var coin))
            CollectCoin(coin);

        if (other.TryGetComponent<Medkit>(out var medkit))
            CollectMedkit(medkit);
    }

    private void CollectCoin(Coin coin)
    {
        _coinSystem?.AddCoin(coin.Value);
        coin.PlayCollectEffect();
        coin.Despawn();
    }

    private void CollectMedkit(Medkit medkit)
    {
        Health _playerHealth = GetComponent<Health>();

        if (_playerHealth != null && _playerHealth.IsAlive)
            _playerHealth.Heal(medkit.HealAmount);

        medkit.PlayHealthEffect();
        medkit.Despawn();
    }
}
