using UnityEngine;
using System.Collections;

/// <summary>
/// Handles collision physics for objects hitting panel
/// </summary>
public class PanelCollision : MonoBehaviour {

    /// <summary>
    /// Enumerator for whic side of the screen the panel is on. 0 = BOTTOM. 1 = TOP.
    /// </summary>
    public enum HEMISPHERE { BOTTOM, TOP };

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
    float addedForceY = 50.0f;
    float newVelocityX = 0.0f;

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
            newVelocityX = ballRigidBody.velocity.y;

            // Get distance from center of panel to contact point
            ContactPoint contact = collision.contacts[0];
            distanceFromCenter = contact.point.x - transform.position.x;
            relativeDistance = distanceFromCenter / lengthFromCenter;

            // Apply magnitude to x velocity, keep y velocity
            newVelocityX *= relativeDistance;
            float velocityY = ballRigidBody.velocity.y;

            // Apply forces to ball
            ballRigidBody.AddForce(0.0f, addedForceY, 0.0f);

            // Must reverse direction for top vs bottom panels
            switch (hemisphere) {
                case HEMISPHERE.BOTTOM:
                    ballRigidBody.velocity = new Vector3(newVelocityX, velocityY, 0);
                    break;
                case HEMISPHERE.TOP:
                    ballRigidBody.velocity = new Vector3(-newVelocityX, velocityY, 0);
                    break;
            }
        }
    }

    // Use this for initialization
    void Start () {
        ballRigidBody = GameObject.FindGameObjectWithTag("ball").GetComponent<Rigidbody>();
        lengthFromCenter = transform.localScale.x / 2.0f;
	}
}
