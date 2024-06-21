using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PermanentDataContainer pData;
    public TemporaryDataContainer tData;

    public GameEvent<Empty> OnSoulChange;
    public MenuAudioEvent MenuEvent;

    [SerializeField] AudioClip BGClip;
    [SerializeField] AudioClip collectClip;

    private void Awake()
    {
        pData.InitializeData = true;
    }
    private void Start()
    {
        MenuEvent.PlayBGMusic.Invoke(BGClip);
    }

    public void CollectItem()
    {
        MenuEvent.ButtonClick.Invoke(collectClip);
    }

    public IEnumerator CountToTarget(int cost)
    {
        int currentSouls = pData.totalSouls + cost;

        int increment = (pData.totalSouls > currentSouls) ? 1 : -1;

        float countingSpeed = Mathf.Abs(cost);

        while (currentSouls != pData.totalSouls)
        {
            currentSouls += increment * Mathf.CeilToInt(countingSpeed * Time.deltaTime);
            // Ensure that we don't overshoot the target
            if ((increment == 1 && currentSouls > pData.totalSouls) || (increment == -1 && currentSouls < pData.totalSouls))
                currentSouls = pData.totalSouls;

            //PlayerGUIManager.Instance.soulCountText.text = currentSouls.ToString();
            OnSoulChange.Raise(new Empty());
            yield return null;
        }
    }
}
