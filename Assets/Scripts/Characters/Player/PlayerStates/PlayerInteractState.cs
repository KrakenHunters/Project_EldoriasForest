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
        player.OnUILoading.OnCancelInteract.Invoke();
    }

    public override void StateFixedUpdate()
    {
        timer += Time.deltaTime;
        player.OnUILoading.OnValueChanged.Invoke(timer, player.interactableObj.waitTime);

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
