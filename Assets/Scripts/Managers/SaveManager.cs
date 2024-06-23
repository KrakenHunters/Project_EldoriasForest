using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public PermanentDataContainer permanentData;
    public TemporaryDataContainer temporaryData;
    public PermanentDataContainer basePermanentData;
    public TemporaryDataContainer baseTemporaryData;

    private string permanentDataFilePath;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        permanentDataFilePath = Application.persistentDataPath + "/GameData.save";
    }

    public void SavePermanentData()
    {
        string jsonData = JsonUtility.ToJson(permanentData);
        if(permanentDataFilePath != null)
        File.WriteAllText(permanentDataFilePath, jsonData);
    }

    public void LoadPermanentData()
    {
        if (File.Exists(permanentDataFilePath))
        {
            string jsonData = File.ReadAllText(permanentDataFilePath);
            JsonUtility.FromJsonOverwrite(jsonData, permanentData);
        }
    }

    public void ResetPermanentData()
    {
        permanentData.totalSouls = basePermanentData.totalSouls;
        permanentData.spellBooksUnlocked = new List<SpecialSpellBook>(basePermanentData.spellBooksUnlocked);
        permanentData.rune = basePermanentData.rune;
        permanentData.cooldownReduction = basePermanentData.cooldownReduction;
        permanentData.healthBonus = basePermanentData.healthBonus;
        permanentData.baseAttackTier = basePermanentData.baseAttackTier;
        permanentData.prefBaseSpell = basePermanentData.prefBaseSpell;
        permanentData.templeSoulsDropRate = basePermanentData.templeSoulsDropRate;
        permanentData.IsUltimateSpellSlotUnlocked = basePermanentData.IsUltimateSpellSlotUnlocked;
        permanentData.InitializeData = basePermanentData.InitializeData;
        SavePermanentData();
    }

    public void ResetTemporaryData()
    {
        temporaryData.collectedSouls = baseTemporaryData.collectedSouls;
        temporaryData.baseSpell = baseTemporaryData.baseSpell;
        temporaryData.specialSpell = null;
        temporaryData.ultimateSpell = null;
        temporaryData.collectedSpells = new List<SpellBook>(baseTemporaryData.collectedSpells);
        temporaryData.startHealth = baseTemporaryData.startHealth;
    }
    

    public void SetUpTempData()
    {
        temporaryData.collectedSouls = 0;
        temporaryData.baseSpell = permanentData.prefBaseSpell;
        temporaryData.ultimateSpell = null;
        temporaryData.collectedSpells = new List<SpellBook>();
        temporaryData.startHealth += permanentData.healthBonus;
    }

    public void TransferTempToPermaData()
    {
        permanentData.totalSouls += temporaryData.collectedSouls;
        foreach (SpecialSpellBook spell in temporaryData.collectedSpells)
        {
            if (!permanentData.spellBooksUnlocked.Contains(spell))
            {
                permanentData.spellBooksUnlocked.Add(spell);
            }
        }
        SavePermanentData();
        ResetTemporaryData();
    }

    public void DeleteAllSaveData()
    {
        if (File.Exists(permanentDataFilePath))
        {
            File.Delete(permanentDataFilePath);
        }
        ResetPermanentData();
        ResetTemporaryData();
    }

    public bool HasSaveData()
    {
        return File.Exists(permanentDataFilePath) && permanentData.InitializeData;
    }

}
