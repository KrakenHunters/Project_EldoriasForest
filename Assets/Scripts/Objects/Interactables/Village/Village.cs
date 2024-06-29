using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Village : Interactable
{
    [SerializeField] private UIPointerEvent TrackEvent;
    protected override void Start()
    {
        base.Start();
        if (!GameManager.Instance.pData.tutorialDone)
        {
            canInteract = false;
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
}
