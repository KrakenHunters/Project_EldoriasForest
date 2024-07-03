using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingImage : MonoBehaviour
{

    [SerializeField] private float blinkSpeed = 1f;
    [SerializeField] private float blinkDuration = 1f;
    private const float DefaultAlpha = 1f;
    private const float FadeAlpha = 0.2f;
    private float timer = 0f;
    private Image image;


    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void StartBlinking()
    {
        StartCoroutine(Blinking());
    }

    private IEnumerator Blinking()
    {
        image.enabled = true;
       while(timer < blinkDuration)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, DefaultAlpha);
            yield return new WaitForSeconds(blinkSpeed);
            image.color = new Color(image.color.r, image.color.g, image.color.b, FadeAlpha);
            yield return new WaitForSeconds(blinkSpeed);
            timer += 1;
        }
       StopBlinking();
    }

    private void StopBlinking()
    {
        StopCoroutine(Blinking());
        image.gameObject.SetActive(false);
    }
    
}
