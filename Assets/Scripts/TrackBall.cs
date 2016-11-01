using UnityEngine;
using System.Collections;

/// <summary>
/// Loosely follows ball
/// </summary>
public class TrackBall : MonoBehaviour {

    /// <summary>
    /// The game's ball object
    /// </summary>
    public GameObject ball;

    /// <summary>
    /// The game's panel object
    /// </summary>
    public GameObject panel;

    /// <summary>
    /// How closely to follow ball's movement
    /// </summary>
    float lookMagnitude = 1.0f / 3.0f;

	// Use this for initialization
	void Start () {
        transform.LookAt(ball.transform.position);
    }
	
	// Update is called once per frame
	void LateUpdate () {

        // If ball falls out of Arena, follow more closely and zoom in
        if (ball.transform.position.y < panel.transform.position.y) {
            lookMagnitude += 0.0025f;
            transform.Translate(0.0f, 0.0f, 0.1f);
        }

        // Loosely follow ball by reducing magnitude of look position
        transform.LookAt(ball.transform.position * lookMagnitude)
    }
}
