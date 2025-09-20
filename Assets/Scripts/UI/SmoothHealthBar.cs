using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : HealthDisplay
{
    private const float MinDifference = 0.001f;

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private float _smoothSpeed = 2f;

    private Coroutine _smoothCoroutine;

    protected override void InitializeDisplay()
    {
        _healthSlider.value = Health.Percentage;
    }

    protected override void OnHealthChanged(float currentHealth)
    {
        if (_smoothCoroutine != null)
            StopCoroutine(_smoothCoroutine);

        _smoothCoroutine = StartCoroutine(SmoothToTarget(Health.Percentage));
    }

    private IEnumerator SmoothToTarget(float targetPercentage)
    {
        while (Mathf.Abs(_healthSlider.value - targetPercentage) > MinDifference)
        {
            _healthSlider.value = Mathf.MoveTowards(_healthSlider.value, targetPercentage, _smoothSpeed * Time.deltaTime);
            yield return null;
        }

        _healthSlider.value = targetPercentage;
        _smoothCoroutine = null;
    }

    private void OnDisable()
    {
        if (_smoothCoroutine != null)
        {
            StopCoroutine(_smoothCoroutine);
            _smoothCoroutine = null;
        }
    }
}
