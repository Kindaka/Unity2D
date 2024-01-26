using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    private float maxHealth = 100f;

    // Start is called before the first frame update
    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        SetMaxHealth(this.maxHealth);
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
