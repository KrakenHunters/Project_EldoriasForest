using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainObjective : Objective
{
    public override void InitializeChallenges()
    {
        challenge1 = new Challenge
        {
            CurrentAmount = 0,
            Goal = 1,
            IsCompleted = false,
            Description = "",
        };
        challenge2 = new Challenge
        {
            CurrentAmount = 0,
            Goal = 100,
            IsCompleted = false,
            Description = ""
        };
        UpdateChallengeDescriptions();
    }
    protected override void UpdateChallengeDescriptions()
    {
        challenge1.Description = $"Main Goal: Kill the Witch";
        challenge2.Description = $"Challenge: Collect {challenge2.CurrentAmount} / {challenge2.Goal} ";
    }

    protected override void CheckObjectiveFinished()
    {
        if(challenge2.IsCompleted)
        {
            challenge2.Description = $"Completed: {challenge2.CurrentAmount} / {challenge2.Goal} souls reward";
            TrackerUIManager.Instance.isChallengeCompleted = true;
        }
        if (challenge1.IsCompleted)
        {
            challenge1.Description = "Return to Village";
            challenge2.Description = "";
        }
    }
}