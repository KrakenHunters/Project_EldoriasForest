using UnityEngine;
using UnityEngine.UI;

public class WSHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private float speed = 2;

    private float targetHealth;
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        healthBarSlider.value = Mathf.Lerp(healthBarSlider.value, targetHealth, speed * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }

    public void SetHealth(float health)
    {
        targetHealth = health;
    }

    public void SetMaxHealth(float maxHealth)
    {
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
        SetHealth(maxHealth);
    }



}
