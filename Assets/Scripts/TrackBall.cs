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
    GameObject panelBottom;

    /// <summary>
    /// The game's bottom panel object
    /// </summary>
    GameObject panelTop;

    /// <summary>
    /// How closely to follow ball's movement
    /// </summary>
    float lookMagnitude = 1.0f / 5.0f;

	// Use this for initialization
	void Start () {
        ball = GameObject.FindGameObjectWithTag("ball");
        panelTop = GameObject.FindGameObjectWithTag("panelTop");
        panelBottom = GameObject.FindGameObjectWithTag("panelBottom");

        transform.LookAt(ball.transform.position);
    }
	
	// Update is called once per frame
	void LateUpdate () {

        // If ball falls out of Arena, follow more closely and zoom in
        if (ball.transform.position.y < panelBottom.transform.position.y) {
            lookMagnitude += 0.0025f;
            transform.Translate(0.0f, 0.0f, 0.1f);
        }

        // If ball falls out of Arena, follow more closely and zoom in
        if (ball.transform.position.y > panelTop.transform.position.y) {
            lookMagnitude += 0.0025f;
            transform.Translate(0.0f, 0.0f, 0.1f);
        }

        // Loosely follow ball by reducing magnitude of look position 
        var target = ball.transform.position;
        transform.LookAt(target * lookMagnitude);
    }
}
