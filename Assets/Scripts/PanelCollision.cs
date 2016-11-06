using UnityEngine;
using System.Collections;

/// <summary>
/// Handles collision physics for objects hitting panel
/// </summary>
public class PanelCollision : MonoBehaviour {

    /// <summary>
    /// Enumerator for whic side of the screen the panel is on. 0 = BOTTOM. 1 = TOP.
    /// </summary>
    public enum HEMISPHERE { LEFT, RIGHT };

    /// <summary>
    /// Enumerator for whic side of the screen the panel is on. 0 = BOTTOM. 1 = TOP.
    /// </summary>
    public HEMISPHERE hemisphere;

    /// <summary>
    /// Physics body for collision and velocity calculations
    /// </summary>
    Rigidbody ballRigidBody;

    /// <summary>
    /// How much force to add to ball each time it hits panel
    /// </summary>
    float addedForceX = 50.0f;
    float newVelocityY = 0.0f;

    /// <summary>
    /// The distance from panel's center when the ball hits the panel (affects x vector)
    /// </summary>
    float distanceFromCenter = 0.0f;

    /// <summary>
    /// The size of Hhlf the panel's width (size of edge to center)
    /// </summary>
    float lengthFromCenter = 0.0f;

    /// <summary>
    /// The ball's distance from center over total length from center (affects x vector)
    /// </summary>
    float relativeDistance = 0.0f;

    /// <summary>
    /// Add x force to object depending on where it hits the panel
    /// </summary>
    /// <param name="collision">Any object with colliding with tag "ricochet"</param>
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("ball")) {

            // Set Y velocity equal to x, and modify based on distance from center
            newVelocityY = ballRigidBody.velocity.x;

            // Get distance from center of panel to contact point
            ContactPoint contact = collision.contacts[0];
            distanceFromCenter = contact.point.y - transform.position.y;
            relativeDistance = distanceFromCenter / lengthFromCenter;

            // Apply magnitude to x velocity, keep y velocity
            newVelocityY *= relativeDistance;
            float velocityX = ballRigidBody.velocity.x;

            // Must reverse direction for top vs bottom panels
            switch (hemisphere) {
                case HEMISPHERE.LEFT:
                    ballRigidBody.AddForce(addedForceX, 0.0f, 0.0f);
                    ballRigidBody.velocity = new Vector3(velocityX, newVelocityY, 0);
                    break;
                case HEMISPHERE.RIGHT:
                    ballRigidBody.AddForce(-addedForceX, 0.0f, 0.0f);
                    ballRigidBody.velocity = new Vector3(velocityX, -newVelocityY, 0);
                    break;
            }
        }
    }

    // Use this for initialization
    void Start () {
        ballRigidBody = GameObject.FindGameObjectWithTag("ball").GetComponent<Rigidbody>();
        lengthFromCenter = transform.localScale.y / 2.0f;
	}
}
