using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective
{
    public Challenge challenge1;
    public Challenge challenge2;
    protected int Reward = 100;
    public virtual void InitializeChallenges()
    {

    }
    protected virtual void UpdateChallengeDescriptions()
    {

    }


    protected virtual void CheckChallengeFinished(Challenge challenge)
    {
        if (challenge.CurrentAmount >= challenge.Goal)
        {
            challenge.IsCompleted = true;
        }
    }
    protected virtual void CheckObjectiveFinished()
    {
        if (challenge1.IsCompleted && challenge2.IsCompleted)
        {
            challenge1.Description = "Return to Base";
            challenge2.Description = "";
            ObjectiveManager.Instance.ObjectiveEvent.OnCompleteObjective.Invoke();
        }
    }
    public void UpdateChallenge(Challenge challenge, int amount)
    {
        challenge.CurrentAmount += amount;
        CheckChallengeFinished(challenge);
        UpdateChallengeDescriptions();
        CheckObjectiveFinished();
    }
    public int GiveReward()
    {
        return Reward;
    }

}


[System.Serializable]
public class Challenge
{
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField] public int CurrentAmount { get; set; }
    [field: SerializeField] public int Goal { get; set; }
    [field: SerializeField] public bool IsCompleted { get; set; }
}
