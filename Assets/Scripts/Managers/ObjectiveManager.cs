using System.Collections;
using UnityEngine;

public class ObjectiveManager : Singleton<ObjectiveManager>
{
    public MainObjective mainObjective = new();
    public TutorialObjective tutorialObjective = new();

    public TemporaryDataContainer tData;
    public PermanentDataContainer pData;

    public ObjectiveEvent ObjectiveEvent;

    private int lastSoulCount = 0;
    private Objective currentObjective;
    private void Awake()
    {
        if (pData.tutorialDone)
        {
            mainObjective.InitializeChallenges();
            currentObjective = mainObjective;
        }
        else
        {
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

    /*    private void OnEnable()
        {
            ObjectiveEvent.OnChallenge1.AddListener(UpdateChallenge1);
            ObjectiveEvent.OnChallenge2.AddListener(UpdateChallenge2);
        }
        private void OnDisable()
        {
            ObjectiveEvent.OnChallenge1.RemoveListener(UpdateChallenge1);
            ObjectiveEvent.OnChallenge2.RemoveListener(UpdateChallenge2);
        }
    */
}



