using System.Collections;
using UnityEngine;

public class DeathCloud : MonoBehaviour
{
    BoxCollider boxCollider;
    private Coroutine damageCoroutine;

    public float targetXPosition = 10f; // The X position where the cloud should stop
    public float moveSpeed = 2f; // The speed at which the cloud moves


    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        // Calculate the leading edge of the box collider in the X direction
        float leadingEdgeX = transform.position.x + (boxCollider.size.x / 2) * transform.localScale.x;

        // Move the cloud towards the target position
        if (leadingEdgeX < targetXPosition)
        {
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, targetXPosition - (boxCollider.size.x / 2) * transform.localScale.x, moveSpeed * Time.deltaTime),
                transform.position.y,
                transform.position.z
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            damageCoroutine = StartCoroutine(ApplyDamageOverTime(other.GetComponent<PlayerController>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            StopCoroutine(damageCoroutine);
        }
    }

    private IEnumerator ApplyDamageOverTime(PlayerController player)
    {
        while (true)
        {
            player.GetHit(player.MaxHealth * 0.25f, this.gameObject, null);
            yield return new WaitForSeconds(2f);
        }
    }
}