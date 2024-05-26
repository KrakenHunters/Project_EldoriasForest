using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple : Interactable
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
        //Code to pop up Menu to select the reward here, als randomizing the values and the objects fr the menu

        //Time.timeScale = 1.0f;
        canInteract = false;
    }
}
