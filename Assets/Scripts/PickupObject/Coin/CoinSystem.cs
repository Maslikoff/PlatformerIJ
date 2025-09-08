using System;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public int TotalCoins { get; private set; } = 0;

    public event Action<int> CoinsUpdated;

    public void Initialize(int initialCoins = 0)
    {
        TotalCoins = initialCoins;
        CoinsUpdated?.Invoke(TotalCoins);
    }

    public void AddCoin(int amount)
    {
        TotalCoins += amount;

        CoinsUpdated?.Invoke(TotalCoins);
    }

    public bool SpendCoins(int amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= amount;
            CoinsUpdated?.Invoke(TotalCoins);

            return true;
        }

        return false;
    }
}
