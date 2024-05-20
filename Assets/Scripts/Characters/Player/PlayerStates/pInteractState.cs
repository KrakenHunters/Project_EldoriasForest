using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class pInteractState : BaseState
{
    PlayerController player;

    private float timer;

    public override void EnterState()
    {
        player = character.GetComponent<PlayerController>();

        //Animate

        timer = 0f;

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {

    }

    public override void StateUpdate()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        //Show UI loader here!

        if (timer >= player.interactableObj.waitTime)
        {
            Debug.Log("Interacting with " + player.interactableObj.name);

            //player.interactableObj.Interact(); //Call interaction with the interactable obj
            player.ChangeState(new pIdleState());
        }
    }

}
