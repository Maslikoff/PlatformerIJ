using System;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public static CoinSystem Instance { get; private set; }

    private int _totalCoins = 0;

    public event Action<int> OnCoinsUpdated;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCoin(int amount)
    {
        _totalCoins += amount;

        OnCoinsUpdated?.Invoke(_totalCoins);
    }

    public int GetTotalCoins()
    {
        return _totalCoins;
    }
}
