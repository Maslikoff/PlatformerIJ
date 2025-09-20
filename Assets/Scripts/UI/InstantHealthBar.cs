using UnityEngine;
using UnityEngine.UI;

public class InstantHealthBar : HealthDisplay
{
    [SerializeField] private Slider _healthSlider;

    protected override void InitializeDisplay()
    {
        UpdateSlider(Health.CurrentValue);
    }

    protected override void OnHealthChanged(float currentHealth)
    {
        UpdateSlider(currentHealth);
    }

    private void UpdateSlider(float currentHealth)
    {
        _healthSlider.value = currentHealth / Health.MaxValue;
    }
}
