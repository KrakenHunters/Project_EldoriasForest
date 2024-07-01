using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform cam; 
    [SerializeField] private LayerMask Layer; 

    private ObjectFader currentObjectToFade;
    private void Awake()
    {
        cam = Camera.main.transform;
        Layer = LayerMask.GetMask("Object");
    }
    private void Update()
    {
        FadeObject();
    }

    private void FadeObject()
    {
        Vector3 playerPos = player.position;

        Ray ray = Camera.main.ScreenPointToRay(playerPos);
        RaycastHit hit;
         
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
        {
         
                Debug.Log("Fading object start stufffffffr");

                if (currentObjectToFade != null)
                {
                    currentObjectToFade.ResetFade();
                }

                currentObjectToFade = hit.collider.GetComponent<ObjectFader>();
                currentObjectToFade.Fade();
        }
    }

   /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (cam == null)
        {
            return;
        }
        Gizmos.DrawRay(cam.position, cam.forward * 100);
    }*/
}
