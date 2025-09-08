using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoinCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Coin>(out var coin))
            CollectCoin(coin);
    }

    private void CollectCoin(Coin coin)
    {
        CoinSystem _coinSystem = FindObjectOfType<CoinSystem>();

        _coinSystem?.AddCoin(coin.Value);
        coin.PlayCollectEffect();
        coin.Despawn();
    }
}
