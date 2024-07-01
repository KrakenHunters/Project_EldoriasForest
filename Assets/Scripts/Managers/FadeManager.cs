using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform cam; 
    [SerializeField] private LayerMask Layer;
    private ObjectFader[] fadeableObjects;
    private ObjectFader currentObjectToFade;

    private void Awake()
    {
        cam = Camera.main.transform;
        Layer = LayerMask.GetMask("Object");
        fadeableObjects = FindObjectsOfType<ObjectFader>();
    }
    private void FixedUpdate()
    {
        FadeObject();
    }

    private void FadeObject()
    {
        Vector3 playerPos = player.position;

        Ray ray = new Ray(Camera.main.transform.position, (player.position - Camera.main.transform.position).normalized);
        RaycastHit[] hits = Physics.SphereCastAll(ray, 2.0f, Mathf.Infinity, Layer);
        foreach (ObjectFader fader in fadeableObjects)
            fader.ShouldFade = false;
        foreach (RaycastHit aHit in hits)
        {
            var fader = aHit.collider.GetComponent<ObjectFader>();
            fader.ShouldFade = true;
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
