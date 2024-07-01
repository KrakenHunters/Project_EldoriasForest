using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField] private Transform player;
    private Vector3 playerOffset;
    [SerializeField] private Transform cam; 
    [SerializeField] private LayerMask Layer;
    private ObjectFader[] fadeableObjects;
    private ObjectFader currentObjectToFade;

    private void Awake()
    {
        Layer = LayerMask.GetMask("Object");
        fadeableObjects = FindObjectsOfType<ObjectFader>();
    }
    private void FixedUpdate()
    {
        FadeObject();
    }

    private void FadeObject()
    {
        playerOffset = new Vector3(player.position.x, player.position.y, player.position.z - 1f);

        Ray ray = new Ray(cam.transform.position, (playerOffset - cam.transform.position).normalized);
        RaycastHit[] hits = Physics.SphereCastAll(ray, 1f, Mathf.Infinity, Layer);
        foreach (ObjectFader fader in fadeableObjects)
            fader.ShouldFade = false;

        foreach (RaycastHit aHit in hits)
        {
            var fader = aHit.collider.GetComponent<ObjectFader>();
            if (fader != null)
            {
                fader.ShouldFade = true;
            }
        }

        foreach (ObjectFader fader in fadeableObjects)
        {

            if (fader.ShouldFade)
                fader.Fade();
            else
                fader.ResetFade();
        }
    }
}
