using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eDie : BaseState
{
    AIController Ai;
    // Start is called before the first frame update
    public override void EnterState()
    {
        Ai = character.GetComponent<AIController>();
        //play death animation
        //play death sound
        //play death particle                                                 
        Ai.Die();   //move to after the animations
    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {

    }

    public override void StateUpdate()
    {

    }
}
