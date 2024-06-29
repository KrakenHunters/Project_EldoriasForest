using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : BaseObject
{
    [SerializeField]
    private float followSpeed = 5f; // Speed at which the collectible follows the player
    [SerializeField]
    private float curveStrength = 2f; // Strength of the lateral force to create the curve


    private Rigidbody rb;
    private PlayerController player;
    private bool isFollowingPlayer = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Apply an initial random force
        Vector3 randomForce = new Vector3(Random.Range(-3f, 3f), Random.Range(1f, 4f), Random.Range(-3f, 3f));
        rb.AddForce(randomForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() && !isFollowingPlayer)
        {
            player = other.GetComponent<PlayerController>();
            if (player.Health > 0f)
            {
                isFollowingPlayer = true;
                StartCoroutine(FollowPlayerCoroutine());
            }
        }
    }

    private IEnumerator FollowPlayerCoroutine()
    {
        while (isFollowingPlayer && player != null)
        {
            // Calculate direction towards the player
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            // Apply force towards the player
            rb.AddForce(directionToPlayer * followSpeed);

            if (Vector3.Distance(transform.position, player.transform.position) >= 10f)
            {
                // Apply a random lateral force to create a curving motion
                Vector3 lateralForce = new Vector3(Random.Range(-curveStrength, curveStrength), 0, Random.Range(-curveStrength, curveStrength));
                rb.AddForce(lateralForce);
            }


            if (Vector3.Distance(transform.position, player.transform.position) < 1f)
            {
                ItemCollected(player);
                Destroy(gameObject);
            }

            yield return new WaitForFixedUpdate(); // Wait for next physics update
        }
    }

    protected virtual void ItemCollected(PlayerController player)
    {
        StopAllCoroutines();
    }

}