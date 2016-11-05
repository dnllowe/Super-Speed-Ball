using UnityEngine;

/// <summary>
/// Moves object by speed and direction each frame toward bounds. Repeats if set to
/// </summary>
public class AutoMove : MonoBehaviour {

    /// <summary>
    /// Whether to return to original position and repeat movement after reaching bounds
    /// </summary>
    public bool repeat = false;

    /// <summary>
    /// How quickly the object moves to x destination
    /// </summary>
    public float speedX = 0.0f;

    /// <summary>
    /// How quickly the object moves to y destination
    /// </summary>
    public float speedY = 0.0f;

    /// <summary>
    /// Controls slow down as object approaches boundaries
    /// </summary>
    public float easing = 1.0f;

    /// <summary>
    /// Amount of easing slow down as object approahces x boundary
    /// </summary>
    public float easingValueX;

    /// <summary>
    /// Amount of easing slow down as object approaches y boundary
    /// </summary>
    public float easingValueY;

    /// <summary>
    /// Direction for x movement
    /// </summary>
    public int directionX = 0;

    /// <summary>
    /// Direction for y movement
    /// </summary>
    public int directionY = 0;

    /// <summary>
    /// Furthest left position to move to
    /// </summary>
    public GameObject leftBoundary;

    /// <summary>
    /// Furthest right position to move to
    /// </summary>
    public GameObject rightBoundary;

    /// <summary>
    /// Furthest top position to move to
    /// </summary>
    public GameObject topBoundary;

    /// <summary>
    /// Furthest bottom position to move to
    /// </summary>
    public GameObject bottomBoundary;

    /// <summary>
    /// Structure to hold object boundaries
    /// </summary>
    struct Boundary {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    /// <summary>
    /// Strucure to hold midpoint of object boundaries
    /// </summary>
    struct Midpoint {
        public float x;
        public float y;
    }

    /// <summary>
    /// Holds object boundaries
    /// </summary>
    Boundary boundary;

    /// <summary>
    /// Holds midpoint of boundaries
    /// </summary>
    Midpoint midpoint;

    // Use this for initialization
    void Start() {
        boundary.left = leftBoundary.transform.position.x;
        boundary.right = rightBoundary.transform.position.x;
        boundary.top = topBoundary.transform.position.y;
        boundary.bottom = bottomBoundary.transform.position.y;
        midpoint.x = (boundary.left + boundary.right) / 2.0f;
        midpoint.y = (boundary.top + boundary.bottom) / 2.0f;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update() {
        
        // Change directions if repeating. Hold in place if not
        if (transform.position.x < boundary.left) {
            transform.position = new Vector3(boundary.left, transform.position.y,
                transform.position.z);
            if (repeat) {
                directionX *= -1;
            } else {
                directionX = 0;
            }
        } else if(transform.position.x > boundary.right) {
            transform.position = new Vector3(boundary.right, transform.position.y,
                transform.position.z);
            if (repeat) {
                directionX *= -1;
            } else {
                directionX = 0;
            }
        }

        if (transform.position.y > boundary.top) {
            transform.position = new Vector3(transform.position.x, boundary.top,
                transform.position.z);
            if (repeat) {
                directionY *= -1;
            } else {
                directionY = 0;
            }
        } else if (transform.position.y < boundary.bottom) {
            transform.position = new Vector3(transform.position.x, boundary.bottom,
                transform.position.z);
            if (repeat) {
                directionY *= -1;
            } else {
                directionY = 0;
            }
        }

        if (transform.position.x < midpoint.x) {
            easingValueX = 1 / (1 +
                    (((midpoint.x - transform.position.x) /
                    (midpoint.x - boundary.left)) * (easing - 1)));
        } else if (transform.position.x > midpoint.x) {
            easingValueX = 1 / (1 +
                    (((transform.position.x - midpoint.x) /
                    (boundary.right - midpoint.x)) * (easing - 1)));
        } else {
            easingValueX = 1;
        }

        if (transform.position.y < midpoint.y) {
            easingValueY = 1 / (1 +
                    (((midpoint.y - transform.position.y) /
                    (midpoint.y - boundary.bottom)) * (easing - 1)));
        } else if (transform.position.y > midpoint.y) {
            easingValueY = 1 / (1 +
                    (((transform.position.y - midpoint.y) /
                    (boundary.top - midpoint.y)) * (easing - 1)));
        } else {
            easingValueY = 1;
        }

        transform.Translate(speedX * directionX * Time.deltaTime * easingValueX, 
                            speedY * directionY * Time.deltaTime * easingValueY, 
                            0.0f, Space.World);
    }
}
