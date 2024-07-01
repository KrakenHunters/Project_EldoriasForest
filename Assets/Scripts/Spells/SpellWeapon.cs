using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator;
    [SerializeField]
    private Color particleColor;

    private PlayerController playerController;

    private void Start()
    {
        if (GetComponent<PlayerController>() != null)
            playerController = GetComponent<PlayerController>();
    }

    public void InstantiateIndicator(SpellBook spell, CharacterClass caster)
    {
        ParticleSystem particleIndicator = indicator.GetComponentInChildren<ParticleSystem>();
        if (spell.castOrigin == SpellBook.castType.skyToGroundPos || spell.castOrigin == SpellBook.castType.groundPos)
        {
            ParticleSystem.MainModule mainModule = particleIndicator.main;
            switch (spell.tier)
            {
                case 1:
                    mainModule.startSize = spell.spellData.tier1.radius;
                    break;
                case 2:
                    mainModule.startSize = spell.spellData.tier2.radius;
                    break;
                case 3:
                    mainModule.startSize = spell.spellData.tier3.radius;
                    break;
                default: break;
            }

            mainModule.startColor = particleColor;
            Instantiate(indicator, caster.spellTarget, Quaternion.identity);
        }

        if (spell is UltimateSpellBook && playerController != null)
        {
            playerController.RemoveUltimateSpell();
        }
    }
}
