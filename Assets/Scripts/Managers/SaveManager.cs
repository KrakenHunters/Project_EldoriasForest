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
        //LoadPermanentData();
    }

    public void SavePermanentData()
    {
        string jsonData = JsonUtility.ToJson(permanentData);
        File.WriteAllText(permanentDataFilePath, jsonData);
    }

    public void LoadPermanentData()
    {
        if (File.Exists(permanentDataFilePath))
        {
            string jsonData = File.ReadAllText(permanentDataFilePath);
            JsonUtility.FromJsonOverwrite(jsonData, permanentData);
        }
       /* else
        {
            ResetPermanentData();
        }*/
    }

    public void ResetPermanentData()
    {
        permanentData.totalSouls = basePermanentData.totalSouls;
        permanentData.spellBooksUnlocked = new List<SpellBook>(basePermanentData.spellBooksUnlocked);
        permanentData.rune = basePermanentData.rune;
        permanentData.cooldownReduction = basePermanentData.cooldownReduction;
        permanentData.healthBonus = basePermanentData.healthBonus;
        permanentData.baseAttackTier = basePermanentData.baseAttackTier;
        permanentData.prefBaseSpell = basePermanentData.prefBaseSpell;
        permanentData.templeSoulsDropRate = basePermanentData.templeSoulsDropRate;
        //SavePermanentData();
    }

    public void ResetTemporaryData()
    {
        temporaryData.collectedSouls = baseTemporaryData.collectedSouls;
        temporaryData.baseSpell = baseTemporaryData.baseSpell;
        temporaryData.specialSpell = baseTemporaryData.specialSpell;
        temporaryData.ultimateSpell = baseTemporaryData.ultimateSpell;
        temporaryData.collectedSpells = new List<SpellBook>(baseTemporaryData.collectedSpells);
        temporaryData.startHealth = baseTemporaryData.startHealth;
    }

    public void TransferTempToPermaData()
    {
        permanentData.totalSouls += temporaryData.collectedSouls;
        foreach (var spell in temporaryData.collectedSpells)
        {
            if (!permanentData.spellBooksUnlocked.Contains(spell))
            {
                permanentData.spellBooksUnlocked.Add(spell);
            }
        }
        SavePermanentData();
    }

    public void DeleteAllSaveData()
    {
        if (File.Exists(permanentDataFilePath))
        {
            File.Delete(permanentDataFilePath);
        }
       /* ResetPermanentData();
        ResetTemporaryData();*/
    }

    public bool HasSaveData()
    {
        return File.Exists(permanentDataFilePath);
    }

    public void SetupTempDataFromPermaData()
    {
        temporaryData.startHealth = permanentData.healthBonus + baseTemporaryData.startHealth;

        // Update other boosts from permanent data as necessary.
    }
}
