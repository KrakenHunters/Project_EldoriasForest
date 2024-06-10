using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerInteractState : BaseState
{

    public override void EnterState()
    {
        base.EnterState();
        //Animate

        timer = 0f;

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        timer += Time.deltaTime;
        Debug.Log(timer);
        //Show UI loader here!

        if (timer >= player.interactableObj.waitTime)
        {
            Debug.Log("Interacting with " + player.interactableObj.name);

            player.interactableObj.Interact(); //Call interaction with the interactable obj
            player.ChangeState(new PlayerMoveState());
        }

    }

    public override void StateUpdate()
    {

    }

    public override void HandleSpecialAttack()
    {

    }

    public override void HandleAttack()
    {

    }


    public override void StopInteract()
    {
        player.ChangeState(new PlayerMoveState());
    }

}
