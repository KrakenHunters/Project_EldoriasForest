using System.Collections;
using UnityEngine;
using static PlayerController;

public class PlayerSpellCastManager : MonoBehaviour
{
    PlayerController player;
    private float baseSpellTimer;
    private float specialSpellTimer;

    public float currentBaseSpellCooldown;
    public float currentSpecialSpellCooldown;

    public float currentSpecialSpellDuration;

    [SerializeField]
    private GameEvent<float> onCooldownChange;


    void Start()
    {
        player = GetComponent<PlayerController>();
        specialSpellTimer = 100f;

    }


    private void Update()
    {
        baseSpellTimer += Time.deltaTime;
        specialSpellTimer += Time.deltaTime;

    }


    private bool CanSpawnBaseSpell()
    {
        if (baseSpellTimer > currentBaseSpellCooldown)
        {
            return true;
        }
        return false;
    }



    private bool CanSpawnSpecialSpell()
    {
        if (specialSpellTimer > currentSpecialSpellCooldown)
        {
            return true;
        }
        return false;
    }

    public void CastBaseSpell()
    {
        if (CanSpawnBaseSpell())
        {
            player.attackType = AttackType.Base;
            player.currentState?.HandleAttack();
            baseSpellTimer = 0f;
        }
    }

    public void CastSpecialSpell()
    {
        if (CanSpawnSpecialSpell())
        {
            player.attackType = AttackType.Special;
            player.currentState?.HandleSpecialAttack();
        }
    }

    public IEnumerator SpecialSpellCooldownTimer()
    {
        specialSpellTimer = 0f;

        while (true)
        {
            float totalTimer = currentSpecialSpellCooldown + currentSpecialSpellDuration;

            if (specialSpellTimer < currentSpecialSpellDuration)
            {
                onCooldownChange.Raise(-1f);
            }
            else if (specialSpellTimer < totalTimer)
            {
                onCooldownChange.Raise(totalTimer - specialSpellTimer);
            }
            else
            {
                onCooldownChange.Raise(0f);
                break;
            }
            yield return null;

        }
    }

    public void CastUltimateSpell()
    {
        player.attackType = AttackType.Ultimate;
        player.currentState?.HandleSpecialAttack();
        player.Invoke("RemoveUltimateSpell", 3f);
    }

}
