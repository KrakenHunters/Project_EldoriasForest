using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageEffect : MonoBehaviour
{
    [SerializeField]
    private float damageIntensity;

    [SerializeField]
    private float damageMaxIntensity;

    [SerializeField]
    private float damageTimer;

    private float playerHealthRatio;

    Volume _volume;
    Vignette _vignette;

    [SerializeField]
    private DoubleFloatEvent gameEvent;

    private void OnEnable()
    {
        gameEvent.OnPlayerGotHit.AddListener(StartDamageEffect);
        gameEvent.OnPlayerHeal.AddListener(ReduceEffect);

    }

    private void OnDisable()
    {
        gameEvent.OnPlayerGotHit.RemoveListener(StartDamageEffect);
    }

    void Start()
    {
        _volume = GetComponent<Volume>();
        if (_volume == null)
        {
            Debug.LogError("Volume component not found.");
            return;
        }

        if (!_volume.profile.TryGet<Vignette>(out _vignette))
        {
            Debug.LogError("Vignette not found in Volume Profile.");
        }
        else
        {
            _vignette.intensity.Override(0f);
        }
    }

    private void StartDamageEffect(float health, float maxHealth)
    {
        playerHealthRatio = health / maxHealth;
        StopAllCoroutines();
        StartCoroutine(TakeDamageEffect());
    }

    private void ReduceEffect(float health, float maxHealth)
    {
        playerHealthRatio = health / maxHealth;
        StopAllCoroutines();
        StartCoroutine(ReduceDamageEffect());
    }

    IEnumerator ReduceDamageEffect()
    {
        float intensity = _vignette.intensity.value;

        yield return null;

        while (intensity > (1 - playerHealthRatio) * damageMaxIntensity)
        {
            intensity -= 0.01f;

            if (intensity < (1 - playerHealthRatio) * damageMaxIntensity)
                intensity = (1 - playerHealthRatio) * damageMaxIntensity;

            _vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(damageTimer);
        }

        yield break;
    }


    IEnumerator TakeDamageEffect()
    {
        float intensity = damageIntensity;
        _vignette.intensity.Override(damageIntensity);

        yield return null;

        while (intensity > (1 - playerHealthRatio) * damageMaxIntensity)
        {
            intensity -= 0.01f;

            if (intensity < (1 - playerHealthRatio) * damageMaxIntensity)
                intensity = (1 - playerHealthRatio) * damageMaxIntensity;

            _vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(damageTimer);
        }

        yield break;
    }
}