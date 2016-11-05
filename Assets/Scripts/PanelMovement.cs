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
    /// Change in touch / mouse drag from player
    /// </summary>
    float deltaX = 0;

    /// <summary>
    /// Vector for latest touch position
    /// </summary>
    Vector3 touchPosition;
 
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
    PlayerInput input;

	// Use this for initialization
	void Start () {
        input = GameObject.FindGameObjectWithTag("input").GetComponent<PlayerInput>();
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        leftBoundary = GameObject.FindGameObjectWithTag("leftBoundary").transform.position.x;
        rightBoundary = GameObject.FindGameObjectWithTag("rightBoundary").transform.position.x;
        touchPosition = transform.position;
    }
	
	// Check whether latest finger moved or mouse click moved and update panel position
	void Update () {

        // Set new position based on touch or keyboard. If no new touch, stay in place.
        if (hemisphere == HEMISPHERE.BOTTOM) {
            deltaX = input.DeltaXBottom;

            if (input.LeftTouchCoordinates != null) {
                touchPosition = oFunctions.ConvertScreenToGameCoordinates(
                    (Vector3)(input.LeftTouchCoordinates), gameObject);
            } else {
                touchPosition = transform.position;
            }
        }

        if (hemisphere == HEMISPHERE.TOP) {
            deltaX = input.DeltaXTop;

            if (input.RightTouchCoordinates != null) {
                touchPosition = oFunctions.ConvertScreenToGameCoordinates(
                    (Vector3)(input.RightTouchCoordinates), gameObject);
            } else {
                touchPosition = transform.position;
            }
        }

        if (SystemInfo.deviceType == DeviceType.Handheld) {

            // Keep panel within bounds, prevent movement if out of bounds 
            var x = touchPosition.x;
            var y = transform.position.y;
            var z = transform.position.z;

            var center = transform.position.x;
            if (x < leftBoundary) {
                x = leftBoundary;
            } else if (x > rightBoundary) {
                x = rightBoundary;
            }
            transform.position = new Vector3(x, y, z);

        } else if (SystemInfo.deviceType == DeviceType.Desktop) {

            // Keep panel within bounds, prevent movement if out of bounds 
            var center = transform.position.x;
            if ((center <= leftBoundary && deltaX < 0) || 
                (center >= rightBoundary && deltaX > 0)) {
                deltaX = 0;
            }
            transform.Translate(deltaX, 0, 0);
        }
        
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
