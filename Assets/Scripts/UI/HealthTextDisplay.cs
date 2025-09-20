using UnityEngine;
using TMPro;

public class HealthTextDisplay : HealthDisplay
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private string _format = "{0}/{1}";

    protected override void InitializeDisplay()
    {
        UpdateDisplay(_health.CurrentHealth);
    }

    protected override void OnHealthChanged(int currentHealth)
    {
        UpdateDisplay(currentHealth);
    }

    private void UpdateDisplay(int currentHealth)
    {
        _healthText.text = string.Format(_format, currentHealth, _health.MaxHealth);
    }
}
