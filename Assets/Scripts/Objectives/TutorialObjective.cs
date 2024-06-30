using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjective : Objective
{
    public override void InitializeChallenges()
    {
        challenge1 = new Challenge
        {
            CurrentAmount = 0,
            Goal = 100,
            IsCompleted = false,
            Description = ""
        };
        challenge2 = new Challenge
        {
            CurrentAmount = 0,
            Goal = 2,
            IsCompleted = false,
            Description = ""
        };
        UpdateChallengeDescriptions();
    }
    protected override void UpdateChallengeDescriptions()
    {
        challenge1.Description = $"Collect {challenge1.CurrentAmount} / {challenge1.Goal} Souls";
        challenge2.Description = $"Collect {challenge2.CurrentAmount} / {challenge2.Goal} Spells from temples";
    }

}
