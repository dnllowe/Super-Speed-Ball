using UnityEngine;
using System.Collections;

/// <summary>
/// Handles panel object response to player input
/// </summary>
public class PanelMovement : MonoBehaviour {

    /// <summary>
    /// Enumerator for whic side of the screen the panel is on. 0 = BOTTOM. 1 = TOP.
    /// </summary>
    public enum HEMISPHERE { LEFT, RIGHT };

    /// <summary>
    /// Enumerator for whic side of the screen the panel is on. 0 = BOTTOM. 1 = TOP.
    /// </summary>
    public HEMISPHERE hemisphere;

    /// <summary>
    /// Starting x vector
    /// </summary>
    public float startY = 0.0f;

    /// <summary>
    /// Change in y based on keypress
    /// </summary>
    float deltaY = 0.0f;

    /// <summary>
    /// Vector for latest touch position
    /// </summary>
    Vector3 touchPosition;
 
    /// <summary>
    /// Left boundary for panel center
    /// </summary>
    float topBoundary = 0.0f;

    /// <summary>
    /// Right boundary for panel center
    /// </summary>
    float bottomBoundary = 0.0f;

    /// <summary>
    /// Provides data on touch / mouse drag and taps / clicks
    /// </summary>
    PlayerInput input;

    /// <summary>
    /// Disable left panel
    /// </summary>
    void DisableLeftPanel() {
        if (hemisphere == HEMISPHERE.LEFT) {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Enable left panel
    /// </summary>
    void EnableLeftPanel() {
        if (hemisphere == HEMISPHERE.LEFT) {
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Disable right panel
    /// </summary>
    void DisableRightPanel() {
        if (hemisphere == HEMISPHERE.RIGHT) {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Enable right panel
    /// </summary>
    void EnableRightPanel() {
        if (hemisphere == HEMISPHERE.RIGHT) {
            gameObject.SetActive(true);
        }
    }

    void OnEnable() {
        PlayerInput.onLeftTouchBegan += EnableLeftPanel;
        PlayerInput.onRightTouchBegan += EnableRightPanel;
        PlayerInput.onLeftTouchEnd += DisableLeftPanel;
        PlayerInput.onRightTouchEnd += DisableRightPanel;
    }

	// Use this for initialization
	void Start () {
        input = GameObject.FindGameObjectWithTag("input").GetComponent<PlayerInput>();
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        topBoundary = GameObject.FindGameObjectWithTag("topBoundary").transform.position.y;
        bottomBoundary = GameObject.FindGameObjectWithTag("bottomBoundary").transform.position.y;
        touchPosition = transform.position;
    }
	
	// Check whether latest finger moved or mouse click moved and update panel position
	void Update () {

        // Set new position based on touch or keyboard. If no new touch, stay in place.
        if (hemisphere == HEMISPHERE.LEFT) {
            deltaY = input.DeltaYLeft;

            if (input.LeftTouchCoordinates != null) {
                touchPosition = oFunctions.ConvertScreenToGameCoordinates(
                    (Vector3)(input.LeftTouchCoordinates), gameObject);
            } else {
                touchPosition = transform.position;
            }
        }

        if (hemisphere == HEMISPHERE.RIGHT) {
            deltaY = input.DeltaYRight;

            if (input.RightTouchCoordinates != null) {
                touchPosition = oFunctions.ConvertScreenToGameCoordinates(
                    (Vector3)(input.RightTouchCoordinates), gameObject);
            } else {
                touchPosition = transform.position;
            }
        }

        if (SystemInfo.deviceType == DeviceType.Handheld) {

            // Keep panel within bounds, prevent movement if out of bounds 
            var x = transform.position.x;
            var y = touchPosition.y;
            var z = transform.position.z;

            if (y < bottomBoundary) {
                y = bottomBoundary;
            } else if (y > topBoundary) {
                y = topBoundary;
            }
            transform.position = new Vector3(x, y, z);

        } else if (SystemInfo.deviceType == DeviceType.Desktop) {

            // Keep panel within bounds, prevent movement if out of bounds 
            var center = transform.position.y;
            if ((center <= bottomBoundary && deltaY < 0) || 
                (center >= topBoundary && deltaY > 0)) {
                deltaY = 0;
            }
            transform.Translate(0, deltaY, 0);
        }
	}
}
