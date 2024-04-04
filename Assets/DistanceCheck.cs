using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's Transform
    private Rigidbody rb; // Reference to this GameObject's Rigidbody

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Calculate the distance between this GameObject and the player
        float distance = Vector3.Distance(player.position, rb.position);

        // If the distance is greater than 6, stop the GameObject by setting its velocity to zero
        if (distance > 6)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
