using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopManager : Singleton<ShopManager>
{
    public TemporaryDataContainer tempData;
    public PermanentDataContainer permData;
    public TMPro.TextMeshProUGUI soulAmountText;

    private float countingSpeed = 50f;

    private void Start()
    {
        //SaveManager.Instance.ResetTemporaryData();
        if (permData.totalSouls != 0)
            StartCoroutine(CountToTarget(-permData.totalSouls));
        else
            soulAmountText.text = permData.totalSouls.ToString();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Renee_ProgrammingGym");
        Debug.Log("Play Game");
        
    }

    public void CheckButtonInteraction(Button button, bool check, int cost)
    {
        button.interactable = check;
        if (cost > 0)
            StartCoroutine(CountToTarget(cost));
    }


    IEnumerator CountToTarget(int cost)
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
}
