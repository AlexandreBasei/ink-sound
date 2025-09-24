using UnityEngine;
using UnityEngine.InputSystem;

public class SoundCollision : MonoBehaviour
{
    public SoundCollisionData Data; // Reference to the SoundCollisionData scriptable object
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to this GameObject
        // Destroy(gameObject, Data.lifetime); // Destroy the GameObject after a certain lifetime
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Vector2 incomingVector = rb.linearVelocity; // Get the incoming vector of the collision
            Vector2 normalVector = collision.contacts[0].normal; // Get the normal vector of the collision
            Vector2 reflectVector = Vector2.Reflect(incomingVector, normalVector); // Calculate the reflection vector

            rb.linearVelocity = reflectVector.normalized * incomingVector.magnitude; // Set the new velocity of the Rigidbody2D
            rb.angularVelocity = 0f; // Reset the angular velocity to prevent spinning

        }
        else if (collision.collider.CompareTag("Ground,NonReflective"))
        {
            Destroy(gameObject); // Destroy the GameObject on collision with any other object
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
