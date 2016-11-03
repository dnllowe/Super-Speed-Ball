using UnityEngine;
using System.Collections;

/// <summary>
/// Handles panel object response to player input
/// </summary>
public class PanelMovement : MonoBehaviour {

    /// <summary>
    /// Enumerator for whic side of the screen the panel is on. 0 = BOTTOM. 1 = TOP.
    /// </summary>
    public enum HEMISPHERE { BOTTOM, TOP };

    /// <summary>
    /// Enumerator for whic side of the screen the panel is on. 0 = BOTTOM. 1 = TOP.
    /// </summary>
    public HEMISPHERE hemisphere;

    /// <summary>
    /// Starting x vector
    /// </summary>
    public float startX = 0.0f;

    /// <summary>
    /// Left boundary object for panel movement
    /// </summary>
    public GameObject leftBoundaryObject;

    /// <summary>
    /// Right boundary object for panel movement
    /// </summary>
    public GameObject rightBoundaryObject;

    /// <summary>
    /// Change in touch / mouse drag from player
    /// </summary>
    float deltaX = 0;

    Vector3 panelPosition;
    int? touchId;

    /// <summary>
    /// Left boundary for panel center
    /// </summary>
    float leftBoundary = 0.0f;

    /// <summary>
    /// Right boundary for panel center
    /// </summary>
    float rightBoundary = 0.0f;

    /// <summary>
    /// Provides data on touch / mouse drag and taps / clicks
    /// </summary>
    public PlayerInput input;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        leftBoundary = leftBoundaryObject.transform.position.x;
        rightBoundary = rightBoundaryObject.transform.position.x;
    }
	
	// Check whether latest finger moved or mouse click moved and update panel position
	void Update () {

        // Clear previously recorded change
        if (hemisphere == HEMISPHERE.BOTTOM) {
            deltaX = input.DeltaXBottom;
            touchId = input.LeftTouch;
        }

        if (hemisphere == HEMISPHERE.TOP) {
            deltaX = input.DeltaXTop;
            touchId = input.RightTouch;
        }

        // Keep panel within bounds, prevent movement if out of bounds 
        var center = transform.position.x;
        var difference = 0.0f;
        if(center <= leftBoundary && deltaX < 0) {
            deltaX = 0;
            difference = leftBoundary - center;
            gameObject.transform.Translate(difference, 0, 0);
        } else if(center >= rightBoundary && deltaX > 0) {
            deltaX = 0;
            difference = rightBoundary - center;
            gameObject.transform.Translate(difference, 0, 0);
        }

        gameObject.transform.Translate(deltaX, 0, 0);
        
	}

    /// <summary>
    /// Set left boundary for panel movement
    /// </summary>
    /// <param name="boundary"></param>
    public void SetLeftBoundary(float boundary) {
        leftBoundary = boundary;
    }

    /// <summary>
    /// Set right boundary for panel movement
    /// </summary>
    /// <param name="boundary"></param>
    public void SetRightBoundary(float boundary) {
        rightBoundary = boundary;
    }

    /// <summary>
    /// Set left and right boundaries for panel movement
    /// </summary>
    /// <param name="boundaryLeft">Left boundary for panel center</param>
    /// <param name="boundaryRight">Right boundary for panel center</param>
    public void SetBoundary(float boundaryLeft, float boundaryRight) {
        leftBoundary = boundaryLeft;
        rightBoundary = boundaryRight;
    }
}
