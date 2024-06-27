using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopManager : Singleton<ShopManager> 
{
    public TemporaryDataContainer tempData;
    public PermanentDataContainer permData;
    public TMPro.TextMeshProUGUI soulAmountText;

    [SerializeField] 
    private GameObject hoverCheck;
    [SerializeField]
    private GameObject shopKeeperManager;

    public Button[] buttonsInScene;

    private ShopBase shopBase;
    private ShopSpecials shopSpecials;
    private ShopCharacterStats shopCharacterStats;

    

    private float countingSpeed = 50f;
    private void Awake()
    {
        shopBase = GetComponent<ShopBase>();
        shopSpecials = GetComponent<ShopSpecials>();
        shopCharacterStats = GetComponent<ShopCharacterStats>();
       SaveManager.Instance.TransferTempToPermaData();
       SaveManager.Instance.ResetTemporaryData();
    }
    private void Start()
    {
        buttonsInScene = FindObjectsOfType<Button>();
        if (permData.totalSouls != 0)
            StartCoroutine(CountToTarget(-permData.totalSouls));
        else
            soulAmountText.text = permData.totalSouls.ToString();
        InvokeRepeating(nameof(AutoSave),3, 20);
    }


    public void PlayGame()
    {
        SaveManager.Instance.SetUpTempData();
        SceneManager.LoadScene("Renee_ProgrammingGym");
    }

    public void CheckButtonInteraction(Button button, bool check)
    {
        button.interactable = check;
    }

    public void CostUIUpdate(int cost)
    {
        if (cost > 0)
            StartCoroutine(CountToTarget(cost));
        SaveManager.Instance.SavePermanentData();

    }

    public void ButtonsInteractability(bool check)
    {
        foreach (Button button in buttonsInScene)
        {
            button.interactable = check;
        }

        if(check)
        {
            shopBase.UpdateButtonInteractions();
            shopCharacterStats.UpdateButtonInteractions();
            shopSpecials.UpdateButtonInteractions();
        }

    }


    public IEnumerator CountToTarget(int cost)
    {
        int currentSouls = permData.totalSouls + cost;

        int increment = (permData.totalSouls > currentSouls) ? 1 : -1;

        countingSpeed = Mathf.Abs(cost);

        while (currentSouls != permData.totalSouls)
        {
            currentSouls += increment * Mathf.CeilToInt(countingSpeed * Time.deltaTime);
            // Ensure that we don't overshoot the target
            if ((increment == 1 && currentSouls > permData.totalSouls) || (increment == -1 && currentSouls < permData.totalSouls))
                currentSouls = permData.totalSouls;

            soulAmountText.text = currentSouls.ToString();

            yield return null;
        }
    }

    private void AutoSave()
    {
        SaveManager.Instance.SavePermanentData();
    }

}

public interface IShoppable
{
    void UpdateButtonInteractions();
    void UpdateSoulsCountUI(int cost);
}
