using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : BaseObject
{
    [HideInInspector]
    public bool canInteract;

    public float waitTime;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        canInteract = true;
    }

    public virtual void Interact()
    {
      //Time.timeScale = 0.0f;
    }

    public void CloseInteraction()
    {
       //Time.timeScale =  1.0f;
    }


}
