using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : Interactable
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
        //Add all the temporary data to permanent data
        //Code to erase current procedural map
        //Load and show village pop up with upgrades and options
        //Button on Menu resets everything back and creates a new procedural map

        //Time.timeScale = 1.0f;
    }
}
