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
            Goal = 30,
            IsCompleted = false,
            Description = ""
        };
        challenge2 = new Challenge
        {
            CurrentAmount = 0,
            Goal = 1,
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

    protected override void CheckObjectiveFinished()
    {
        if (challenge1.IsCompleted && challenge2.IsCompleted)
        {
            challenge1.Description = "Find Portal to Village";
            challenge2.Description = "";
            ObjectiveManager.Instance.ObjectiveEvent.OnCompleteObjective.Invoke();
            ObjectiveManager.Instance.ObjectiveEvent.OnCompleteTutorialObjectives.Invoke();
            TrackerUIManager.Instance.isChallengeCompleted = true;
        }
    }
}
