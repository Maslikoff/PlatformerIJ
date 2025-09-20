using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : HealthDisplay
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private float _smoothSpeed = 2f;

    private Coroutine _smoothCoroutine;

    protected override void InitializeDisplay()
    {
        _healthSlider.value = _health.HealthPercentage;
    }

    protected override void OnHealthChanged(int currentHealth)
    {
        if (_smoothCoroutine != null)
            StopCoroutine(_smoothCoroutine);

        _smoothCoroutine = StartCoroutine(SmoothToTarget(_health.HealthPercentage));
    }

    private IEnumerator SmoothToTarget(float targetPercentage)
    {
        while (Mathf.Abs(_healthSlider.value - targetPercentage) > 0.001f)
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
