using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public PermanentDataContainer permanentData;
    public TemporaryDataContainer temporaryData;
    public PermanentDataContainer basePermanentData;
    public TemporaryDataContainer baseTemporaryData;

    public SpecialSpellBook[] specialSpellBooks; 
    public BaseSpellBook[] baseSpellBooks;

    private Dictionary<int, SpecialSpellBook> specialSpellBookDictionary = new Dictionary<int, SpecialSpellBook>();
    private Dictionary<int, BaseSpellBook> baseSpellBookDictionary = new Dictionary<int, BaseSpellBook>();

    private string permanentDataFilePath;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        permanentDataFilePath = Path.Combine(Application.persistentDataPath, "GameData.json");

        InitializeSpellBookDictionaries();
    }

    private void InitializeSpellBookDictionaries()
    {
        for (int i = 0; i < specialSpellBooks.Length; i++)
        {
            if (!specialSpellBookDictionary.ContainsKey(i))
            {
                specialSpellBookDictionary.Add(i, specialSpellBooks[i]);
            }
        }

        for (int i = 0; i < baseSpellBooks.Length; i++)
        {
            if (!baseSpellBookDictionary.ContainsKey(i))
            {
                baseSpellBookDictionary.Add(i, baseSpellBooks[i]);
            }
        }
    }

    public void SavePermanentData()
    {
        List<int> spellBookIDs = new List<int>();
        foreach (SpecialSpellBook spell in permanentData.spellBooksUnlocked)
        {
            int id = GetSpellBookID(spell);
            if (id >= 0)
            {
                spellBookIDs.Add(id);
            }
        }
        permanentData.spellBookIDs = spellBookIDs;

        try
        {
            string json = JsonUtility.ToJson(permanentData, true);
            File.WriteAllText(permanentDataFilePath, json);
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to save data to {permanentDataFilePath}: {ex.Message}");
        }
    }

    public void LoadPermanentData()
    {
        if (File.Exists(permanentDataFilePath))
        {
            try
            {
                string json = File.ReadAllText(permanentDataFilePath);
                JsonUtility.FromJsonOverwrite(json, permanentData);
                RestoreDataAfterLoading();
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to load data from {permanentDataFilePath}: {ex.Message}");
            }
        }
    }

    private int GetSpellBookID(SpecialSpellBook spell)
    {
        foreach (var kvp in specialSpellBookDictionary)
        {
            if (kvp.Value == spell)
            {
                return kvp.Key;
            }
        }
        return -1;
    }

    private void RestoreDataAfterLoading()
    {
        List<SpecialSpellBook> loadedSpells = new List<SpecialSpellBook>();
        foreach (int id in permanentData.spellBookIDs)
        {
            if (specialSpellBookDictionary.TryGetValue(id, out SpecialSpellBook spell))
            {
                loadedSpells.Add(spell);
            }
        }
        permanentData.spellBooksUnlocked = loadedSpells;
    }

    public void ResetPermanentData()
    {
        permanentData.totalSouls = basePermanentData.totalSouls;
        permanentData.spellBooksUnlocked.Clear();
        permanentData.spellBooksUnlocked.AddRange(basePermanentData.spellBooksUnlocked);
        permanentData.rune = basePermanentData.rune;
        permanentData.cooldownReduction = basePermanentData.cooldownReduction;
        permanentData.healthBonus = basePermanentData.healthBonus;
        permanentData.baseAttackTier = basePermanentData.baseAttackTier;
        permanentData.prefBaseSpell = basePermanentData.prefBaseSpell;
        permanentData.templeSoulsDropRate = basePermanentData.templeSoulsDropRate;
        permanentData.IsUltimateSpellSlotUnlocked = basePermanentData.IsUltimateSpellSlotUnlocked;
        permanentData.InitializeData = basePermanentData.InitializeData;
        permanentData.tutorialDone = basePermanentData.tutorialDone;

        permanentData.baseUpgradeCost = basePermanentData.baseUpgradeCost;
        permanentData.healthUpgradeCost = basePermanentData.healthUpgradeCost;
        permanentData.defensiveRuneCost = basePermanentData.defensiveRuneCost;
        permanentData.soulDropUpgradeCost  = basePermanentData.soulDropUpgradeCost;
        permanentData.spellCooldownCost  = basePermanentData.spellCooldownCost;

        

        permanentData.templeSoulsDropRateIncrement = basePermanentData.templeSoulsDropRateIncrement;
        permanentData.runeIncrement = basePermanentData.runeIncrement;
        permanentData.cooldownReductionIncrement = basePermanentData.cooldownReductionIncrement;
        permanentData.healthBonusIncrement = basePermanentData.healthBonusIncrement;
    }

    public void ResetTemporaryData()
    {
        temporaryData.collectedSouls = baseTemporaryData.collectedSouls;
        temporaryData.baseSpell = baseTemporaryData.baseSpell;
        temporaryData.specialSpell = null;
        temporaryData.ultimateSpell = null;
        temporaryData.collectedSpells.Clear();
        temporaryData.collectedSpells.AddRange(baseTemporaryData.collectedSpells);
        temporaryData.startHealth = baseTemporaryData.startHealth;
    }

    public void SetUpTempData()
    {
        temporaryData.collectedSouls = 0;
        temporaryData.baseSpell = permanentData.prefBaseSpell;
        temporaryData.ultimateSpell = null;
        temporaryData.collectedSpells.Clear();
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
        return File.Exists(permanentDataFilePath);
    }
}
