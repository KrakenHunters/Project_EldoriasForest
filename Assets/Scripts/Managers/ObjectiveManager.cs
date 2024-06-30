using System.Collections;
using UnityEngine;

public class ObjectiveManager : Singleton<ObjectiveManager>
{
    public MainObjective mainObjective;
    public TutorialObjective tutorialObjective;

    [SerializeField] private int reward = 100;

    public TemporaryDataContainer tData;
    public PermanentDataContainer pData;

    public ObjectiveEvent ObjectiveEvent;
    public GameEvent<Empty> OnSoulCollected;

    private int lastSoulCount = 0;
    private bool rewardGiven = false;
    private Objective currentObjective;
    private void Awake()
    {

        if (pData.tutorialDone)
        {
            mainObjective = new MainObjective();
            mainObjective.InitializeChallenges();
            currentObjective = mainObjective;
            Debug.Log("Main Objective");
        }
        else
        {
            tutorialObjective = new TutorialObjective();
            tutorialObjective.InitializeChallenges();
            currentObjective = tutorialObjective;
        }
        ObjectiveEvent.SetObjective.Invoke(currentObjective);
        ObjectiveEvent.OnUpdateObjective.Invoke();
    }

    
    public void SoulsCollected() // Called when the player collects a soul event
    {
        if (!GameManager.Instance.pData.tutorialDone)
        {
            UpdateChallenge1(tData.collectedSouls - lastSoulCount);

            lastSoulCount = tData.collectedSouls;
        }
        else
        {
            UpdateChallenge2(tData.collectedSouls - lastSoulCount);
            lastSoulCount = tData.collectedSouls;
            if(currentObjective.challenge2.IsCompleted && !rewardGiven)
            {
                rewardGiven = true;
                tData.collectedSouls += reward;
                OnSoulCollected.Raise(new Empty());
            }
        }
        ObjectiveEvent.OnUpdateObjective.Invoke();
    }

    public void SpellCollected() // Called when the player collects a spell event
    {
        if (!pData.tutorialDone)
        {
            UpdateChallenge2(1);
        }
        ObjectiveEvent.OnUpdateObjective.Invoke();
    }

    public void KillWitch()
    {
        if (pData.tutorialDone)
        {
            UpdateChallenge1(1);
        }
        ObjectiveEvent.OnUpdateObjective.Invoke();
    }  // Called when the player kills the witch

    private void UpdateChallenge1(int amount)
    {
        currentObjective.UpdateChallenge(currentObjective.challenge1, amount);
        ObjectiveEvent.OnUpdateObjective.Invoke();

    }

    private void UpdateChallenge2(int amount)
    {
        Debug.Log($"current objective is" + currentObjective.challenge2.CurrentAmount);
        currentObjective.UpdateChallenge(currentObjective.challenge2, amount);
        ObjectiveEvent.OnUpdateObjective.Invoke();
    }

   
}



