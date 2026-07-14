using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image HealthBar;

    private void OnEnable()
    {
        PlayerController.OnHealthChange += UpdateHealthBar;
    }

    private void Disable()
    {
        PlayerController.OnHealthChange -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float healthRatio)
    {
        HealthBar.fillAmount = healthRatio;
    }
}
