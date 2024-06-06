using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    public Slider HealthBarSlider;
    public Image sliderFill;

    public Material greenEmission;
    public Material yellowEmission;
    public Material redEmission;

    private float targetHealthPercentage;
    private float smoothSpeed = 0.1f; // Adjust the speed to your preference

    private void Start()
    {
        targetHealthPercentage = HealthBarSlider.value;
    }

    private void Update()
    {
        if (Mathf.Abs(HealthBarSlider.value - targetHealthPercentage) > 0.01f)
        {
            HealthBarSlider.value = Mathf.Lerp(HealthBarSlider.value, targetHealthPercentage, Time.deltaTime / smoothSpeed);
        }
    }

    public void UpdateSliderValue(float currentHealth, float maxHealth)
    {
        // Calculate the health percentage
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);

        // Set the target value for the slider
        targetHealthPercentage = healthPercentage;

        // Update the color based on health percentage
        UpdateColor(healthPercentage);
    }

    private void UpdateColor(float healthPercentage)
    {
        if (healthPercentage >= 0.6f)
        {
            sliderFill.material = greenEmission;
        }
        else if (healthPercentage >= 0.3f)
        {
            sliderFill.material = yellowEmission;
        }
        else
        {
            sliderFill.material = redEmission;
        }
    }
}