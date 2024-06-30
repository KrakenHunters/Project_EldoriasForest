using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ObjectiveEvent", menuName = "Events/Objective Event")]
public class ObjectiveEvent : SOEvent
{
    public UnityEvent<Objective> SetObjective;
    public UnityEvent OnUpdateObjective;
    public UnityEvent OnCompleteObjective;
    public UnityEvent<int> OnChallenge1;
    public UnityEvent<int> OnChallenge2;
}
