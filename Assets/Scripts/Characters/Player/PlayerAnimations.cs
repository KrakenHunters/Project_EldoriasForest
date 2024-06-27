using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField]  LayerMask groundLayer;

    private Animator _anim;
    private Vector2 _input;

    private void Awake() => _anim = GetComponentInChildren<Animator>();

    private void Update()
    {
      AimToMouse(); 
        


        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(_input.x, 0, _input.y);

        if (move.magnitude > 0)
        {
          move.Normalize();
            move *= speed * Time.deltaTime;
            transform.Translate(move,Space.World);
        }


        //animat

        float velocityZ = Vector3.Dot(move.normalized, transform.forward);
        float velocityX = Vector3.Dot(move.normalized, transform.right);

        _anim.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _anim.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
    }

    private void AimToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookAt);
        }
    }

}
