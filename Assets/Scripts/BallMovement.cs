using UnityEngine;
using System.Collections;

/// <summary>
/// Controls movement of Ball object
/// </summary>
public class BallMovement : MonoBehaviour {

    BallProperties ballProperties;

    /// <summary>
    /// The bottom panel in the game level
    /// </summary>
    GameObject panelBottom;

    /// <summary>
    /// The top panel in the game level
    /// </summary>
    GameObject panelTop;

    /// <summary>
    /// Score keeper for game
    /// </summary>
    ScoreKeeper scoreKeeper;

    /// <summary>
    /// Left boundary for panel center
    /// </summary>
    float leftBoundary = 0.0f;

    /// <summary>
    /// Right boundary for panel center
    /// </summary>
    float rightBoundary = 0.0f;

    /// <summary>
    /// Physics body for collision and velocity calculations
    /// </summary>
    Rigidbody rigidBody;

    /// <summary>
    /// The fastest recorded velocity of the ball. Used to prevent it from going below
    /// </summary>
    float maxSpeed = 0.0f;

    /// <summary>
    /// The fastest recorded velocity of the ball. Used to prevent it from going below
    /// </summary>
    public float MaxSpeed {
        get { return maxSpeed; }
    }

    /// <summary>
    /// Used to display velocity vector in Unity
    /// </summary>
    public Vector2 displayVelocity;

    /// <summary>
    /// How much to increase speed after each boost
    /// </summary>
    float speedIncrease = 0.0f;
  
    public void IncreaseMaxSpeed() {
        maxSpeed *= speedIncrease;
    }

    /// <summary>
    /// Updates the max recorded velocity if it changes
    /// </summary>
    public void UpdateMaxSpeed() {
        var currentSpeed = 0.0f;
        currentSpeed = (Mathf.Abs(rigidBody.velocity.y) >
                Mathf.Abs(rigidBody.velocity.x)) ?
                Mathf.Abs(rigidBody.velocity.y) : Mathf.Abs(rigidBody.velocity.x);
        if (currentSpeed > maxSpeed) {
            maxSpeed = currentSpeed;
        }
    }
    
    /// <summary>
    /// Make sure the fastest current vector is always going at least the max recorded vector
    /// </summary>
    public void MaintainMaxSpeed() {

        float velocityY = rigidBody.velocity.y;
        float velocityX = rigidBody.velocity.x;
        float speedY = Mathf.Abs(velocityY);
        float speedX = Mathf.Abs(velocityX);

        // We need to know which vector to apply max Velocity to and which direction it's heading
        int direction = 0;

        if(speedY > speedX) {
            direction = (velocityY > 0) ? direction = 1 : direction = -1;
            rigidBody.velocity = new Vector3(velocityX, maxSpeed * direction, 0.0f);
        } else {
            direction = (velocityX > 0) ? direction = 1 : direction = -1;
            rigidBody.velocity = new Vector3(maxSpeed * direction, velocityY, 0.0f);
        }
    }

	// Gravity is not enough. Push ball down at start
	void Start () {
        ballProperties = GetComponent<BallProperties>();
        transform.position = new Vector3(ballProperties.startX, 
            transform.position.y, transform.position.z);
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(0.0f, -ballProperties.InitialForce, 0.0f);
        speedIncrease = 1.05f; 
        leftBoundary = ballProperties.leftBoundaryObject.transform.position.x;
        rightBoundary = ballProperties.rightBoundaryObject.transform.position.x;
        panelBottom = ballProperties.panelBottom;
        rightBoundary = ballProperties.rightBoundaryObject.transform.position.x;
        panelTop = ballProperties.panelTop;
        scoreKeeper = ballProperties.scoreKeeper;
    }

    void Update() {
        if ((transform.position.y < panelBottom.transform.position.y ||
            transform.position.y > panelTop.transform.position.y ||
            transform.position.x < leftBoundary ||
            transform.position.x > rightBoundary) && !scoreKeeper.IsGameOver) {
            GetComponent<Collider>().enabled = false;
            scoreKeeper.GetTimer().StartTimer();
            scoreKeeper.GetTimer().SetMark(scoreKeeper.RestartTime);
            scoreKeeper.IsGameOver = true;
        }

        displayVelocity = rigidBody.velocity;
    }

    void FixedUpdate() {
        // NOTE: if using Dash feature, the Dash script will handle FixedUpdate
        UpdateMaxSpeed();
        MaintainMaxSpeed();
    }
}
