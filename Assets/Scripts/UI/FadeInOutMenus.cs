using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class FadeInOutMenus : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Tween fadeTween;
    [SerializeField] private float fadeDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fade(float endValue, float fadeDuration, TweenCallback onEnd)
    {
        if(fadeTween != null)
        {
            fadeTween.Kill(false);
        }
        fadeTween = canvasGroup.DOFade(endValue, fadeDuration);
    }

  }
