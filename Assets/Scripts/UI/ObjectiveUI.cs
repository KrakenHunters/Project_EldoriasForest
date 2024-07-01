using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Challenge1checkMark;
    public GameObject Challenge2checkMark;

    public TextMeshProUGUI challenge1Text;
    public TextMeshProUGUI challenge2Text;

    public Objective objective;
    public ObjectiveEvent objectiveEvent;

    public void UpdateUI()
    {

        challenge1Text.text = objective.challenge1.Description;
        challenge2Text.text = objective.challenge2.Description;

        Challenge1checkMark.SetActive(objective.challenge1.IsCompleted);
        Challenge2checkMark.SetActive(objective.challenge2.IsCompleted);

        challenge1Text.color = objective.challenge1.IsCompleted ? Color.green : Color.white;
        challenge2Text.color = objective.challenge2.IsCompleted ? Color.green : Color.white;
    }

    private void SetObjective(Objective obj)
    {
        objective = obj;
    }

    private void OnEnable()
    {
        objectiveEvent.SetObjective.AddListener(SetObjective);
        objectiveEvent.OnUpdateObjective.AddListener(UpdateUI);
        objectiveEvent.OnCompleteObjective.AddListener(UpdateUI);
    }

    private void OnDisable()
    {
        objectiveEvent.SetObjective.RemoveListener(SetObjective);
        objectiveEvent.OnUpdateObjective.RemoveListener(UpdateUI);
        objectiveEvent.OnCompleteObjective.RemoveListener(UpdateUI);
    }
}
