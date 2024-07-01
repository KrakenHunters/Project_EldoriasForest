using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material fadeableMaterial;

    [SerializeField] private float fadeSpeed;

    public bool ShouldFade { get; set; }
    private const float DefaultOpacity = 1f;
    private const float FadeOpacity = .2f;
    private float _opacity = DefaultOpacity;
    private Material mat;
    Color currentColor;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        currentColor = mat.color;
    }

    void FixedUpdate()
    {
        if (ShouldFade && _opacity > FadeOpacity)
        {
            mat = fadeableMaterial;
            GetComponent<Renderer>().material = mat;
            _opacity -= fadeSpeed * Time.fixedDeltaTime;
            mat.color = new Color(currentColor.r, currentColor.g, currentColor.b, _opacity);
        }
        else if (!ShouldFade && _opacity < DefaultOpacity)
        {
            mat = fadeableMaterial;
            GetComponent<Renderer>().material = mat;
            _opacity += fadeSpeed * Time.fixedDeltaTime;
            mat.color = new Color(currentColor.r, currentColor.g, currentColor.b, _opacity);
        }
        else if (!ShouldFade && _opacity >= DefaultOpacity)
        {
            mat = originalMaterial;
            GetComponent<Renderer>().material = mat;
        }
    }

    public void Fade()
    {
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b,
            Mathf.Lerp(currentColor.a, FadeOpacity, fadeSpeed * Time.deltaTime));
    }

    public void ResetFade()
    {
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b,
            Mathf.Lerp(currentColor.a, DefaultOpacity, fadeSpeed * Time.deltaTime));
    }

}
