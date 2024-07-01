using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{

    [SerializeField] private float fadeSpeed;
    [SerializeField] private float fadeAmount;

    private const float DefaultOpacity = 1f;
    private Material mat;
    Color currentColor;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        currentColor = mat.color;
    }

    public void Fade()
    {
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b,
            Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
        mat.color = newColor;
    }

    public void ResetFade()
    {
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b,
            Mathf.Lerp(currentColor.a, DefaultOpacity, fadeSpeed * Time.deltaTime));
        mat.color = newColor;
    }

}
