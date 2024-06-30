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
            Goal = 15,
            IsCompleted = false,
            Description = ""
        };
        UpdateChallengeDescriptions();
    }
    protected override void UpdateChallengeDescriptions()
    {
        challenge1.Description = $"Kill the Witch";
        challenge2.Description = $"Collect {challenge2.CurrentAmount} / {challenge2.Goal} Souls";
    }

    protected override void CheckObjectiveFinished()
    {
        if (challenge1.IsCompleted)
        {
            challenge1.Description = "Return to Base";
        }
            
        if(challenge2.IsCompleted)
        {
            challenge2.Description = "Completed: 100 souls reward";
        }
    }
}