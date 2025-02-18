using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerInteractState : BaseState
{

    public override void EnterState()
    {
        base.EnterState();
        player.animator.CrossFade(InteractHash, 0.2f);

        timer = 0f;

    }
    public override void ExitState()
    {
        player.doubleFloatEvent.OnCancelInteract.Invoke();
    }

    public override void StateFixedUpdate()
    {
        timer += Time.deltaTime;
        player.doubleFloatEvent.OnValueChanged.Invoke(timer, player.interactableObj.waitTime);

        if (timer >= player.interactableObj.waitTime)
        {
            player.interactableObj.Interact(); //Call interaction with the interactable obj
            player.interactableObj = null;

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
