using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour {

    public Text nameText;

    public Slider healthSlider;
    public Text healthText;

    public void SetHUD(Unit unit) {

        nameText.text = unit.unitName;

        healthSlider.maxValue = unit.maxHealth;
        healthSlider.value = unit.currentHealth;
        healthText.text = $"{unit.currentHealth}";

    }

    public void SetHealth(int health) {
        healthSlider.value = health;
        healthText.text = $"{health}";
    }

}
