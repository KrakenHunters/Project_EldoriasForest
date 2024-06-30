using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Village : Interactable
{
    [SerializeField] private UIPointerEvent TrackEvent;

    public ObjectiveEvent OnObjectiveEvent;
    protected override void Start()
    {
        base.Start();
        if (!GameManager.Instance.pData.tutorialDone)
        {
            canInteract = false;
        }
        else
        {
            canInteract = true;
        }
    }

    public override void Interact()
    {
        base.Interact();
        Time.timeScale = 1f;
        SceneManager.LoadScene("01_Shop");
        TrackEvent.EndTargetTracking.Invoke();
    }

    public void TriggerTracker()
    {
        TrackEvent.SendTargetPos.Invoke(transform);
    }

    public void EndTracker()
    {
        TrackEvent.EndTargetTracking.Invoke();
    }

    private void ActivateVillage()
    {
        canInteract = true;
    }

    private void OnEnable()
    {
        OnObjectiveEvent.OnCompleteObjective.AddListener(ActivateVillage);
    }
    private void OnDisable()
    {
        OnObjectiveEvent.OnCompleteObjective.RemoveListener(ActivateVillage);
    }

}
