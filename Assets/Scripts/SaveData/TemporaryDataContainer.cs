using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data Containers/TemporaryData")]

public class TemporaryDataContainer : ScriptableObject
{
    public int collectedSouls;
    public BaseSpellBook baseSpell;
    public SpecialSpellBook specialSpell;
    public UltimateSpellBook ultimateSpell;
    public List<SpellBook> collectedSpells;

    public int startHealth;

   
}
