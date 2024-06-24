using UnityEngine;
using UnityEngine.UI;

public class WSHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private float speed = 2;
    [SerializeField] private bool isWitch = false;

    private float targetHealth;
    private Camera mainCamera;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        //transform.LookAt(2 * transform.position - Camera.main.transform.position);
        healthBarSlider.value = Mathf.Lerp(healthBarSlider.value, targetHealth, speed * Time.deltaTime);
        if (!isWitch)
        {
            Vector3 direction = (mainCamera.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(-direction);
            lookRotation.eulerAngles = new Vector3(lookRotation.eulerAngles.x, 0, 0);
            transform.rotation = lookRotation;
        }
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
