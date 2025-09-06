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

    public bool SpendCoins(int amount)
    {
        if (_totalCoins >= amount)
        {
            _totalCoins -= amount;
            OnCoinsUpdated?.Invoke(_totalCoins);

            return true;
        }

        return false;
    }

    public int GetTotalCoins()
    {
        return _totalCoins;
    }
}
