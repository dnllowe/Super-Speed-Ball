using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    Touch? leftTouch = null;
    Touch? rightTouch = null;
    Touch? latestTouch = null;

    /// <summary>
    /// Sensitivity of touch / mouse drag input. Set on Start()
    /// </summary>
    float sensitivity = 0.0f;

    /// <summary>
    /// Sensitivity for desktop / mouse input
    /// </summary>
    public float keyboardSensitivity = 1.0f;

    /// <summary>
    /// Sensitivity for touch / mobile input
    /// </summary>
    public float touchSensitivity = 0.5f;

    /// <summary>
    /// Change in touch / mouse drag since previous frame
    /// </summary>
    float deltaXBottom = 0.0f;

    /// <summary>
    /// Change in touch / mouse drag since previous frame
    /// </summary>
    public float DeltaXBottom {
        get { return deltaXBottom; }
    }

    /// <summary>
    /// Change in touch / mouse drag since previous frame
    /// </summary>
    float deltaXTop = 0.0f;

    /// <summary>
    /// Change in touch / mouse drag since previous frame
    /// </summary>
    public float DeltaXTop {
        get { return deltaXTop; }
    }

    /// <summary>
    /// Initial touch location of rapid double tap
    /// </summary>
    Vector2 initialTouchLocation = new Vector2(0.0f, 0.0f);

    /// <summary>
    /// Final touch location of rapid double tap
    /// </summary>
    Vector2 latestTouchLocation = new Vector2(0.0f, 0.0f);

    /// <summary>
    /// Normalized vector for touch1 - touch0
    /// </summary>
    Vector2 deltaTouch = new Vector2(1.0f, 1.0f);

    /// <summary>
    /// Normalized vector for touch1 - touch0
    /// </summary>
    public Vector2 DeltaTouch {
        get { return deltaTouch; }
    }

    /// <summary>
    /// Delegate for single finger double tap
    /// </summary>
    public delegate void OnDoubleTap();

    /// <summary>
    /// Event callback double tap
    /// </summary>
    public static event OnDoubleTap onDoubleTap;

    /// <summary>
    /// Enumerator for double tap state. NONE = no recent taps. BEGAN = first tap
    /// </summary>
    enum DoubleTapState { NONE, BEGAN };

    /// <summary>
    /// Enumerator for double tap state. NONE = no recent taps. INITIAL = first tap
    /// </summary>
    DoubleTapState doubleTapState;

    /// <summary>
    /// Enumerator for multi-touch state. NONE = no simultaneous touches. MULTI = more than one touch. TO_SINGLE = transition from multi to single touch
    /// </summary>
    enum MultiTouchState { NONE, MULTI, TO_SINGLE };

    /// <summary>
    /// Enumerator for multi-touch state. NONE = no simultaneous touches. MULTI = more than one touch. TO_SINGLE = transition from multi to single touch
    /// </summary>
    MultiTouchState multiTouchState;

    /// <summary>
    /// Delegate for two or more touches simultaneously
    /// </summary>
    public delegate void OnMultiTouchBegan();

    /// <summary>
    /// Event callback for two or more touches simultaneously
    /// </summary>
    public static event OnMultiTouchBegan onMultiTouchBegan;

    /// <summary>
    /// Delegate for multi-touch transition to single touch
    /// </summary>
    public delegate void OnMultiToSingle();

    /// <summary>
    /// Event callback for multi-touch transition to single touch
    /// </summary>
    public static event OnMultiToSingle onMultiToSingle;

    /// <summary>
    /// Delegate for multi-touch ending
    /// </summary>
    public delegate void OnMultiTouchEnd();

    /// <summary>
    /// Event callback for multi-touch ending
    /// </summary>
    public static event OnMultiTouchEnd onMultiTouchEnd;

    /// <summary>
    /// Timer to track double taps and other time-based input
    /// </summary>
    oTimer timer;

    /// <summary>
    /// Gets difference in tap / mouse x position when pressed / LMB held
    /// </summary>
    void UpdateDeltaX() {

        deltaXBottom = 0.0f;
        deltaXTop = 0.0f;

        if (SystemInfo.deviceType == DeviceType.Handheld) {
            // Find the latest touch
            if (Input.touchCount > 0 &&
            Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Began) {
                latestTouch = Input.GetTouch(Input.touchCount - 1);
            } 

            // Assign latest touch to the appropriate side of the screen
            if (latestTouch.HasValue &&
                latestTouch.Value.phase == TouchPhase.Began &&
                latestTouch.Value.position.y < Screen.height / 2) {
                leftTouch = latestTouch.Value;
            }

            if (latestTouch.HasValue &&
                latestTouch.Value.phase == TouchPhase.Began &&
                latestTouch.Value.position.y > Screen.height / 2) {
                rightTouch = latestTouch.Value;
            }

            if (leftTouch.HasValue) {
                deltaXBottom = leftTouch.Value.deltaPosition.x * sensitivity;
            }

            if (rightTouch.HasValue) {
                deltaXTop = rightTouch.Value.deltaPosition.x * sensitivity;
            }

            // Reset until next new touch
            latestTouch = null;

        } else if (SystemInfo.deviceType == DeviceType.Desktop) {

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W)) {
                deltaXBottom = -1 * sensitivity;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
                deltaXBottom = 1 * sensitivity;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow)) {
                deltaXTop = -1 * sensitivity;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow)) {
                deltaXTop = 1 * sensitivity;
            }
        }
    }

    /// <summary>
    /// Updates normalized vector for touch1 - touch0
    /// </summary>
    void UpdateDeltaTouch() {
        deltaTouch = latestTouchLocation - initialTouchLocation;
    }

    /// <summary>
    /// Updates touch positions and checks if double touch occurred (taps within 500 ms)
    /// </summary>
    void CheckDoubleTaps() {

        if (SystemInfo.deviceType == DeviceType.Handheld) {
            if (doubleTapState == DoubleTapState.BEGAN && 
                timer.GetElapsedTime() > 500) {
                doubleTapState = DoubleTapState.NONE;
                timer.ResetTimer();
            }
            // First make sure there are touches to process
            if (Input.touchCount > 0) {
                var touch = Input.GetTouch(Input.touchCount - 1);
                if (touch.phase == TouchPhase.Began) {
                    if (doubleTapState == DoubleTapState.BEGAN) {
                        if(onDoubleTap  != null) {
                            onDoubleTap();
                        }
                        doubleTapState = DoubleTapState.NONE;
                        timer.ResetTimer();
                    } else if (doubleTapState == DoubleTapState.NONE) {
                        doubleTapState = DoubleTapState.BEGAN;
                        timer.RestartTimer();
                    }
                    initialTouchLocation = latestTouchLocation;
                    latestTouchLocation = new Vector2(touch.position.x, touch.position.y); 
                }
            }
        } else if (SystemInfo.deviceType == DeviceType.Desktop) {
            if (doubleTapState == DoubleTapState.BEGAN 
                && timer.GetElapsedTime() > 500) {
                doubleTapState = DoubleTapState.NONE;
                timer.ResetTimer();
            }
            if (Input.GetKeyDown(KeyCode.Space)) {           
                if (doubleTapState == DoubleTapState.BEGAN) {
                    if (onDoubleTap != null) {
                        onDoubleTap();
                    }
                    doubleTapState = DoubleTapState.NONE;
                    timer.ResetTimer();
                } else if (doubleTapState == DoubleTapState.NONE) {
                    doubleTapState = DoubleTapState.BEGAN;
                    timer.RestartTimer();
                }
                var click = Input.mousePosition;
                initialTouchLocation = latestTouchLocation;
                latestTouchLocation = new Vector2(click.x, click.y);
            }
        }
    }

    /// <summary>
    /// Check state of multi-touch
    /// </summary>
    void CheckMultiTouches() {

        if (SystemInfo.deviceType == DeviceType.Handheld) {
            if (multiTouchState != MultiTouchState.MULTI && Input.touchCount >= 2) {
                multiTouchState = MultiTouchState.MULTI;
                if (onMultiTouchBegan != null) {
                    onMultiTouchBegan();
                }
            } else if (multiTouchState == MultiTouchState.MULTI && Input.touchCount == 1) {
                multiTouchState = MultiTouchState.TO_SINGLE;
                if (onMultiToSingle != null) {
                    onMultiToSingle();
                }
            } else if (multiTouchState != MultiTouchState.NONE && Input.touchCount == 0) {
                multiTouchState = MultiTouchState.NONE;
                if (onMultiTouchEnd != null) {
                    onMultiTouchEnd();
                }
            }
        } else if(SystemInfo.deviceType == DeviceType.Desktop) {
            if (multiTouchState != MultiTouchState.MULTI && 
                Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1)) {
                multiTouchState = MultiTouchState.MULTI;
                if (onMultiTouchBegan != null) {
                    onMultiTouchBegan();
                }
            } else if (multiTouchState == MultiTouchState.MULTI && 
                Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1)) {
                multiTouchState = MultiTouchState.TO_SINGLE;
                if (onMultiToSingle != null) {
                    onMultiToSingle();
                }
            } else if (multiTouchState != MultiTouchState.NONE && 
                !Input.GetKey(KeyCode.Mouse0)) {
                multiTouchState = MultiTouchState.NONE;
                if (onMultiTouchEnd != null) {
                    onMultiTouchEnd();
                }
            }
        }
    }

    void Start () {
        if (SystemInfo.deviceType == DeviceType.Handheld) {
            sensitivity = touchSensitivity;
        } else if (SystemInfo.deviceType == DeviceType.Desktop) {
            sensitivity = keyboardSensitivity;
        }

        doubleTapState = DoubleTapState.NONE;
        multiTouchState = MultiTouchState.NONE;
        timer = gameObject.AddComponent<oTimer>();
    }
	
    void Update() {
        CheckDoubleTaps();
        CheckMultiTouches();
    }
	// Update is called once per frame
	void FixedUpdate () {
        UpdateDeltaX();
	}
}
