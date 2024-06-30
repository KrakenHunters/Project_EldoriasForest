using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective
{
    public Challenge challenge1;
    public Challenge challenge2;

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
        }
    }

    public void UpdateChallenge(Challenge challenge, int amount)
    {
        challenge.CurrentAmount += amount;
        CheckChallengeFinished(challenge);
        UpdateChallengeDescriptions();
        CheckObjectiveFinished();
    }
}

[System.Serializable]
public struct Challenge
{
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField] public int CurrentAmount { get; set; }
    [field: SerializeField] public int Goal { get; set; }
    [field: SerializeField] public bool IsCompleted { get; set; }
}
