using UnityEngine;
using UnityEngine.UI;

public class InstantHealthBar : HealthDisplay
{
    [SerializeField] private Slider _healthSlider;

    protected override void InitializeDisplay()
    {
        UpdateSlider(_health.CurrentHealth);
    }

    protected override void OnHealthChanged(int currentHealth)
    {
        UpdateSlider(currentHealth);
    }

    private void UpdateSlider(int currentHealth)
    {
        _healthSlider.value = (float)currentHealth / _health.MaxHealth;
    }
}
