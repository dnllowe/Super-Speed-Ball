using UnityEngine;
using System.Collections;

/// <summary>
/// Loosely follows ball
/// </summary>
public class TrackBall : MonoBehaviour {

    /// <summary>
    /// The game's ball object
    /// </summary>
    GameObject ball;

    /// <summary>
    /// The game's bottom panel object
    /// </summary>
    GameObject panelLeft;

    /// <summary>
    /// The game's bottom panel object
    /// </summary>
    GameObject panelRight;

    /// <summary>
    /// How closely to follow ball's movement
    /// </summary>
    float lookMagnitude = 1.0f / 5.0f;

	// Use this for initialization
	void Start () {
        ball = GameObject.FindGameObjectWithTag("ball");
        panelLeft = GameObject.FindGameObjectWithTag("panelLeft");
        panelRight = GameObject.FindGameObjectWithTag("panelRight");

        transform.LookAt(ball.transform.position);
    }
	
	// Update is called once per frame
	void LateUpdate () {

        // If ball falls out of Arena, follow more closely and zoom in
        if (ball.transform.position.x < panelLeft.transform.position.x) {
            lookMagnitude += 0.0025f;
            transform.Translate(0.0f, 0.0f, 0.1f);
        }

        // If ball falls out of Arena, follow more closely and zoom in
        if (ball.transform.position.x > panelRight.transform.position.x) {
            lookMagnitude += 0.0025f;
            transform.Translate(0.0f, 0.0f, 0.1f);
        }

        // Loosely follow ball x position by reducing magnitude of look position 
        var target = ball.transform.position;
        target = new Vector3(target.x, 0, 0);
        transform.LookAt(target * lookMagnitude);
    }
}
