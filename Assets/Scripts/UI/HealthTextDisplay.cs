using UnityEngine;
using TMPro;

public class HealthTextDisplay : HealthDisplay
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private string _format = "{0}/{1}";

    protected override void InitializeDisplay()
    {
        UpdateDisplay(Health.CurrentValue);
    }

    protected override void OnHealthChanged(float currentHealth)
    {
        UpdateDisplay(currentHealth);
    }

    private void UpdateDisplay(float currentHealth)
    {
        _healthText.text = string.Format(_format, currentHealth, Health.MaxValue);
    }
}
