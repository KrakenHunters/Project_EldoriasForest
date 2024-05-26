using UnityEngine;
using static PlayerController;

public class PlayerSpellCastManager : Singleton<PlayerSpellCastManager>
{
    PlayerController player;
    public float baseSpellTimer;
    public float specialSpellTimer;
    public float ultimateSpellTimer;

    public float currentBaseSpellCooldown;
    public float currentSpecialSpellCooldown;
    public float currentUltimateSpellCooldown;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }


    private void Update()
    {
        baseSpellTimer += Time.deltaTime;
        specialSpellTimer += Time.deltaTime;
        ultimateSpellTimer += Time.deltaTime;
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

    private bool CanSpawnUltimateSpell()
    {
        if (ultimateSpellTimer > currentUltimateSpellCooldown)
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
            player.currentState?.HandleAttack();
            specialSpellTimer = 0f;
        }
    }

    public void CastUltimateSpell()
    {
        if (CanSpawnUltimateSpell())
        {
            player.attackType = AttackType.Ultimate;
            player.currentState?.HandleAttack();
            ultimateSpellTimer = 0f;
            player.Invoke("RemoveUltimateSpell", 3f);
        }
    }

}
