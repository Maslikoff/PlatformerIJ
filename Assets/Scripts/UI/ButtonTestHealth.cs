using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTestHealth : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private int _damageAmount = 10;
    [SerializeField] private int _healAmount = 10;

    public void SimulateDamage()
    {
        _health.TakeDamage(_damageAmount);
    }

    public void SimulateHeal()
    {
        _health.Heal(_healAmount);
    }
}
