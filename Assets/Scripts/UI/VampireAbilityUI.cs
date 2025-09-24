using UnityEngine;
using UnityEngine.UI;

public class VampireAbilityUI : MonoBehaviour
{
    [SerializeField] private VampireAbility _vampireAbility;

    [Header("UI References")]
    [SerializeField] private Slider _abilitySlider;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _backgroundImage;

    [Header("Colors")]
    [SerializeField] private Color _activeColor = Color.red;
    [SerializeField] private Color _readyColor = Color.green;
    [SerializeField] private Color _cooldownColor = Color.gray;

    private bool _isInCooldownMode = false;

    private void Awake()
    {
        InitializeUI();
    }

    private void OnEnable()
    {
        _vampireAbility.AbilityProgressChanged += UpdateAbilityProgress;
        _vampireAbility.CooldownProgressChanged += UpdateCooldownProgress;
        _vampireAbility.AbilityStateChanged += UpdateVisualState;
    }

    private void OnDisable()
    {
        _vampireAbility.AbilityProgressChanged -= UpdateAbilityProgress;
        _vampireAbility.CooldownProgressChanged -= UpdateCooldownProgress;
        _vampireAbility.AbilityStateChanged -= UpdateVisualState;
    }

    private void InitializeUI()
    {
        _abilitySlider.value = 1f;
        UpdateVisualState(false);
    }

    private void UpdateAbilityProgress(float progress)
    {
        if (_isInCooldownMode == false)
            _abilitySlider.value = progress;
    }

    private void UpdateCooldownProgress(float progress)
    {
        if (_isInCooldownMode)
            _abilitySlider.value = progress;

        if (progress >= 0.99f)
            UpdateVisualState(false);
    }

    private void UpdateVisualState(bool isAbilityActive)
    {
        _isInCooldownMode = isAbilityActive == false && _vampireAbility.IsOnCooldown;

        if (isAbilityActive)
        {
            _fillImage.color = _activeColor;
            _backgroundImage.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            _abilitySlider.value = 1f;
        }
        else if (_vampireAbility.IsOnCooldown)
        {
            _fillImage.color = _cooldownColor;
            _backgroundImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            _abilitySlider.value = 0f;
        }
        else
        {
            _fillImage.color = _readyColor;
            _backgroundImage.color = new Color(0.1f, 0.4f, 0.1f, 0.3f);
            _abilitySlider.value = 1f;
        }
    }
}