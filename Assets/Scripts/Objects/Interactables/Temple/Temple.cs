using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temple : Interactable
{
    private SphereCollider sphereCollider;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        canInteract = true;
        sphereCollider = GetComponent<SphereCollider>();
    }

    public override void Interact()
    {
       base.Interact();
        //Code to pop up Menu to select the reward here, als randomizing the values and the objects fr the menu
        TempleUIManager.Instance.SetTempleOptions(tier);
        DeactivateInteractable();
        sphereCollider.enabled = false;
        canInteract = false;
        
    }

}


