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
            Description = $"Kill the Witch",
        };
        challenge2 = new Challenge
        {
            CurrentAmount = 0,
            Goal = 100,
            IsCompleted = false,
            Description = $"Collect {challenge2.CurrentAmount} / {challenge2.Goal} Souls"
        };
        UpdateChallengeDescriptions();
    }
    protected override void UpdateChallengeDescriptions()
    {
        challenge1.Description = $"Kill the Witch";
        challenge2.Description = $"Collect {challenge2.CurrentAmount} / {challenge2.Goal} Souls";
    }
}