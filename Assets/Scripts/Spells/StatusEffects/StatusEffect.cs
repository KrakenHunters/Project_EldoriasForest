using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class StatusEffect : MonoBehaviour
{
    private CharacterClass _target;
    [SerializeField]
    private Element element;

    [SerializeField]
    private ParticleSystem _particles;
    private enum Element
    {
        LightningEffect,
        FireEffect,
        IceEffect,
        None
    }


    private void Awake()
    {
        _target = GetComponentInParent<CharacterClass>();
    }

    public void ActivateStatusEffect(float timer, float value)
    {
        switch (element)
        {
            case Element.IceEffect:
                StartCoroutine(OnFrozen(timer, value));
                break;
            case Element.FireEffect:
                StartCoroutine(OnBurning(timer, value));
                break;
            case Element.LightningEffect:
                StartCoroutine(OnStunned(timer));
                break;
            default:
                break;


        }

    }

    private IEnumerator OnFrozen(float effectTimer, float damageMult)
    {
        float timer = 0f;
        _target.damageMultiplier = damageMult;

        while (timer < effectTimer)
        {
            timer += 1f;
            _particles.Play();
            yield return new WaitForSeconds(1f);
        }
        DisableStausEffect();


    }

    private IEnumerator OnStunned(float effectTimer)
    {
        float timer = 0f;
        _target.Speed = 0f;

        while (timer < effectTimer)
        {
            timer += 1f;
            _particles.Play();

            yield return new WaitForSeconds(1f);
        }
        DisableStausEffect();

    }

    public void DisableStausEffect()
    {
        _target.Speed = _target.initialSpeed;
        _target.damageMultiplier = 1f;
        Destroy(gameObject, _particles.main.duration);

    }


    private IEnumerator OnBurning(float effectTimer, float damage)
    {
        float timer = 0f;

        while (timer < effectTimer)
        {
            yield return new WaitForSeconds(1f);

            timer += 1f;
            _particles.Play();

            _target.GetHit(damage, this.gameObject, null);


        }
        DisableStausEffect();

    }
}
