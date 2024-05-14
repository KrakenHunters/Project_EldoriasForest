using UnityEngine;
public class pIdleState : BaseState
{
    public override void HandleMovement(Vector2 d)
    {
        character.ChangeState(new pMoveState());
    }
}
