using UnityEngine;

public class BallProperties : MonoBehaviour {

    /// <summary>
    /// Starting x vector
    /// </summary>
    public float startY = 0.0f;

    /// <summary>
    /// How much force hits the ball to start each round
    /// </summary>
    public float initialForce = 2000.0f;

    /// <summary>
    /// How much force hits the ball to start each round
    /// </summary>
    public float InitialForce {
        get { return initialForce; }
    }

    /// <summary>
    /// Threshold for change in touch position that registers as different touch location
    /// </summary>
    public float deltaTouchThreshold = 5.0f;
}
