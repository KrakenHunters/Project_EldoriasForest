using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : Singleton<GameManager>
{
    public PermanentDataContainer pData;
    public TemporaryDataContainer tData;

    public GameEvent<Empty> OnSoulChange;
    public MenuAudioEvent MenuEvent;

    public EnemyAudioEvent enemyEvent;
    public BossEnemy witch;

    [SerializeField] PlayableDirector witchCinematic;

    [SerializeField] AudioClip BGClip;
    [SerializeField] AudioClip collectClip;

    [SerializeField] GameObject ScreamUI;

    public bool enemyActions;

    private void Awake()
    {
        pData.InitializeData = true;
    }
    private void Start()
    {
        if (!pData.tutorialDone)
        {
            StartCoroutine(BlinkScreamUI());
            enemyActions = false;
        }
        else
        {
            enemyActions = true;
        }
        MenuEvent.PlayBGMusic.Invoke(BGClip);
    }

    public void StartEnemyActions()
    {
        enemyActions = true;
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

    private IEnumerator BlinkScreamUI()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 3 seconds before starting the blink effect
        enemyEvent.OnWitchScream.Invoke(witch);

        float elapsedTime = 0f;
        float blinkDuration = 1f;

        while (elapsedTime < blinkDuration)
        {
            ScreamUI.SetActive(!ScreamUI.activeSelf);
            yield return new WaitForSeconds(0.5f); // Adjust the duration to control the blink speed
            elapsedTime += 0.5f;
        }

        ScreamUI.SetActive(false); // Ensure ScreamUI is inactive after blinking
        witchCinematic.Play();
    }
}
