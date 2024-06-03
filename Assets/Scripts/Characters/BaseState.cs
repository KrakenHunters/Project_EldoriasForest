using UnityEngine;

public abstract class BaseState
{

    protected float nextDirection;
    protected Vector3 _direction;
    protected LayerMask groundLayer = LayerMask.GetMask("Ground");
    public CharacterClass character { get; set; }
    public InputManager inputManager { get; set; }

    protected BaseState currentState;

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateUpdate() { }
    public virtual void HandleMovement(Vector2 dir) { }
    public virtual void HandleAttack() { }
    public virtual void HandleInteract() { }
    public virtual void StopInteract() { }

    public virtual void HandleDeath() { }

    protected virtual void RotateToTarget()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 target = hit.point;
            Vector3 direction = target - character.transform.position;
            direction.y = 0;
            character.gameObject.transform.rotation = Quaternion.Slerp(character.gameObject.transform.rotation, Quaternion.LookRotation(direction), character.RotationSpeed);
        }
    }
}