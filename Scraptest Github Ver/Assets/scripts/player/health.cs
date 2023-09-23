using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    public float currentHP;
    private float maxHealth;

    public float currentEnergy;
    private float maxEnergy;

    public Image HealthBar;
    public Image EnergyBar;
    public int barType; // 0 for hp. 1 for energy

    public bool playerOne;

    private void Update()
    {
        if (playerOne)
        {
            if (barType == 0)
            {
                maxHealth = demoscript.maxHP;
                currentHP = player.currentHealth;
                HealthBar.fillAmount = currentHP / maxHealth;
            }
            else if (barType == 1)
            {
                maxEnergy = demoscript.maxEnergy;
                currentEnergy = player.currentEnergy;
                EnergyBar.fillAmount = currentEnergy / maxEnergy;
            }
        }
        if (!playerOne)
        {
            if (barType == 0)
            {
                maxHealth = demoscript.maxHP;
                currentHP = player.currentHealth_player2;
                HealthBar.fillAmount = currentHP / maxHealth;
            }
            else if (barType == 1)
            {
                maxEnergy = demoscript.maxEnergy;
                currentEnergy = player.currentEnergy_player2;
                EnergyBar.fillAmount = currentEnergy / maxEnergy;
            }
        }
    }


}
