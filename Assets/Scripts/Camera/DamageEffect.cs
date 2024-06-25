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
        gameEvent.OnPlayerHeal.AddListener(StartHealEffect);
    }

    private void OnDisable()
    {
        gameEvent.OnPlayerGotHit.RemoveListener(StartDamageEffect);
        gameEvent.OnPlayerHeal.RemoveListener(StartHealEffect);
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
        StartCoroutine(AdjustDamageEffect());
    }

    private void StartHealEffect(float health, float maxHealth)
    {
        playerHealthRatio = health / maxHealth;
        StopAllCoroutines();
        StartCoroutine(AdjustDamageEffect());
    }

    IEnumerator AdjustDamageEffect()
    {
        float targetIntensity = (1 - playerHealthRatio) * damageMaxIntensity;
        float currentIntensity = _vignette.intensity.value;

        if (currentIntensity < targetIntensity)
        {
            // Healing
            while (currentIntensity < targetIntensity)
            {
                currentIntensity += 0.05f;
                _vignette.intensity.Override(currentIntensity);

                yield return new WaitForSeconds(damageTimer);
            }
        }
        else
        {
            // Taking Damage
            while (currentIntensity > targetIntensity)
            {
                currentIntensity -= 0.05f;
                _vignette.intensity.Override(currentIntensity);

                yield return new WaitForSeconds(damageTimer);
            }
        }

        yield break;
    }
}