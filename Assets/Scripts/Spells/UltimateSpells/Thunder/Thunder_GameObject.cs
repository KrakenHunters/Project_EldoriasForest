using UnityEngine;

public class Thunder_GameObject : MonoBehaviour
{
    [SerializeField]
    private float fallSpeed = 15f;  // Speed at which the thunder effect falls

    private void Update()
    {
        // Move the thunder effect downward
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the thunder effect hit the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Destroy the thunder effect upon hitting the ground
        }
    }
}
