using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Village : Interactable
{
    [SerializeField] private float checkDistance = 25f;
    [SerializeField] private UIPointerEvent TrackEvent;
    private Transform player;
    private float distance;
    protected override void Start()
    {
         player = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
        Time.timeScale = 1f;
        SceneManager.LoadScene("01_Shop");
        TrackEvent.EndTargetTracking.Invoke();
    }

    private void Update()
    {
         distance = Vector3.Distance(player.position, transform.position);
        if (distance < checkDistance)
        {
            TrackEvent.SendTargetPos.Invoke(transform);
        }
    }
}
