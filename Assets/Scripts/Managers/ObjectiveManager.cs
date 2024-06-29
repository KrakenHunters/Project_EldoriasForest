using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : Singleton<ObjectiveManager>
{
    [Header("Objective system")]
    [SerializeField] private Objective Main;

    [Header("Objective UI")]
    [SerializeField] private TextMeshPro MainObjectiveUI;
    [SerializeField] private TextMeshPro SecondaryObjectiveUI;
    [SerializeField] private TextMeshPro TutorialObjectiveUI;

    private void Awake()
    {
        Main.soulsToCollect = 5;
        Main.soulsCollected = 0;
        Main.objectiveDescription = "Collect 5 souls";
        Main.returnToVillage = "Return to the village";

    }

    private void SetObjectiveUI()
    {

    }

}

[System.Serializable]
public class Objective
{
    public  int soulsToCollect;
    public  int soulsCollected;
    public string objectiveDescription;
    public string returnToVillage;

}

