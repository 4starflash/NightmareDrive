using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image EnergyBar;

    private void OnEnable()
    {
        PlayerController.OnHealthChange += UpdateHealthBar;
        PlayerController.OnEnergyChange += UpdateEnergyBar;
    }

    private void Disable()
    {
        PlayerController.OnHealthChange -= UpdateHealthBar;
        PlayerController.OnEnergyChange -= UpdateEnergyBar;
    }

    private void UpdateHealthBar(float healthRatio)
    {
        HealthBar.fillAmount = healthRatio;
    }

    private void UpdateEnergyBar(float energyRatio)
    {
        EnergyBar.fillAmount = energyRatio;
    }
}
