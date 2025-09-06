using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;

    private void Start()
    {
        _coinText.text = CoinSystem.Instance.GetTotalCoins().ToString();

        CoinSystem.Instance.OnCoinsUpdated += UpdateCoinText;
    }

    private void OnDisable()
    {
        if (CoinSystem.Instance != null)
            CoinSystem.Instance.OnCoinsUpdated -= UpdateCoinText;
    }

    private void UpdateCoinText(int amount)
    {
        _coinText.text = $"Coins: {amount}";
    }
}
