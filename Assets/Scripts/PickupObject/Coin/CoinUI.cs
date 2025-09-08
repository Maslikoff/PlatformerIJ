using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private CoinSystem _coinSystem;

    private void Start()
    {
        _coinText.text = _coinSystem.TotalCoins.ToString();
        _coinSystem.OnCoinsUpdated += UpdateCoinText;
    }

    private void OnDisable()
    {
        if (_coinSystem != null)
            _coinSystem.OnCoinsUpdated -= UpdateCoinText;
    }

    private void UpdateCoinText(int amount)
    {
        _coinText.text = $"Coins: {amount}";
    }
}
