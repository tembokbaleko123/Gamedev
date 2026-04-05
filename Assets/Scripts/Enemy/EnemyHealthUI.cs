using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Health health;
    public Slider healthSlider;

    void Update()
    {
        if (health != null)
        {
            healthSlider.maxValue = health.maxHealth;
            healthSlider.value = health.GetCurrentHealth();
        }
    }
}