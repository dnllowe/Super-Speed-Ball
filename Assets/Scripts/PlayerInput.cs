using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour {

    /// <summary>
    /// The most recent left touch. Null if no touch on left side of screen
    /// </summary>
    int? leftTouchId = null;

    /// <summary>
    /// The most recent right touch. Null if no touch on right side of screen
    /// </summary>
    int? rightTouchId = null;

    /// <summary>
    /// The most recent left touch. Null if no touch on left side of screen
    /// </summary>
    public int? LeftTouchId {
        get { return leftTouchId; }
    }

    /// <summary>
    /// The most recent right touch. Null if no touch on right side of screen
    /// </summary>
    public int? RightTouchId {
        get { return rightTouchId; }
    }

    /// <summary>
    /// The left touch x screen coordinate
    /// </summary>
    float? leftTouchX = null;

    /// <summary>
    /// The left touch y screen coordinate
    /// </summary>
    float? leftTouchY = null;

    /// <summary>
    /// The right touch x screen coordinate
    /// </summary>
    float? rightTouchX = null;

    /// <summary>
    /// The right touch y screen coordinate
    /// </summary>
    float? rightTouchY = null;

    /// <summary>
    /// The left touch x screen coordinate
    /// </summary>
    public float? LeftTouchX {
        get { return leftTouchX; }
    }

    /// <summary>
    /// The left touch y screen coordinate
    /// </summary>
    public float? LeftTouchY {
        get { return leftTouchY; }
    }

    /// <summary>
    /// The right touch x screen coordinate
    /// </summary>
    public float? RightTouchX {
        get { return rightTouchX; }
    }

    /// <summary>
    /// The right touch y screen coordinate
    /// </summary>
    public float? RightTouchY {
        get { return rightTouchY; }
    }

    /// <summary>
    /// Vector for left touch x and y screen coordinates
    /// </summary>
    Vector3? leftTouchCoordinates = null;

    /// <summary>
    /// Vector for right touch x and y screen coordinates
    /// </summary>
    Vector3? rightTouchCoordinates = null;

    /// <summary>
    /// Vector for left touch x and y screen coordinates
    /// </summary>
    public Vector3? LeftTouchCoordinates {
        get { return leftTouchCoordinates; }
    }

    /// <summary>
    /// Vector for right touch x and y screen coordinates
    /// </summary>
    public Vector3? RightTouchCoordinates {
        get { return rightTouchCoordinates; }
    }

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
    public float touchSensitivity = 0.01f;

    /// <summary>
    /// Change in touch / mouse drag since previous frame
    /// </summary>
    float deltaYLeft = 0.0f;

    /// <summary>
    /// Change in touch / mouse drag since previous frame
    /// </summary>
    public float DeltaYLeft {
        get { return deltaYLeft; }
    }

    /// <summary>
    /// Change in touch / mouse drag since previous frame
    /// </summary>
    float deltaYRight = 0.0f;

    /// <summary>
    /// Change in touch / mouse drag since previous frame
    /// </summary>
    public float DeltaYRight {
        get { return deltaYRight; }
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

    oTimer debugTimer;

    /// <summary>
    /// Gets difference in tap / mouse x position when pressed / LMB held
    /// </summary>
    void UpdateDeltaX() {

        deltaYLeft = 0.0f;
        deltaYRight = 0.0f;

        if (SystemInfo.deviceType == DeviceType.Handheld) {

            // Loop through touches to update left and right touch changes
            for (int iii = 0; iii < Input.touchCount; iii++) {

                var touch = Input.GetTouch(iii);

                // Assign latest touches to screen hemispheres
                if (touch.phase == TouchPhase.Began &&
                    touch.position.x < Screen.width / 2) {
                    leftTouchId = touch.fingerId;
                }

                if (touch.phase == TouchPhase.Began &&
                    touch.position.x > Screen.width / 2) {
                    rightTouchId = touch.fingerId;
                }

                if (leftTouchId == touch.fingerId) {
                    deltaYLeft = touch.position.y;
                    leftTouchX = touch.position.x;
                    leftTouchY = touch.position.y;
                    leftTouchCoordinates = new Vector3(touch.position.x, touch.position.y, 0);

                    if (touch.phase == TouchPhase.Ended) {
                        leftTouchId = null;
                        leftTouchX = null;
                        leftTouchY = null;
                        leftTouchCoordinates = null;
                    }
                }

                if (rightTouchId == touch.fingerId) {
                    deltaYRight = touch.position.y;
                    rightTouchX = touch.position.x;
                    rightTouchY = touch.position.y;
                    rightTouchCoordinates = new Vector3(touch.position.x, touch.position.y, 0);

                    if (touch.phase == TouchPhase.Ended) {
                        rightTouchId = null;
                        rightTouchX = null;
                        rightTouchY = null;
                        rightTouchCoordinates = null;
                    }
                }
            }

        } else if (SystemInfo.deviceType == DeviceType.Desktop) {

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W)) {
                deltaYLeft = 1 * sensitivity;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
                deltaYLeft = -1 * sensitivity;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow)) {
                deltaYRight = 1 * sensitivity;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow)) {
                deltaYRight = -1 * sensitivity;
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

    void Awake() {
        timer = gameObject.AddComponent<oTimer>();
        debugTimer = gameObject.AddComponent<oTimer>();
    }

    void Start () {

        if (SystemInfo.deviceType == DeviceType.Handheld) {
            sensitivity = touchSensitivity;
        } else if (SystemInfo.deviceType == DeviceType.Desktop) {
            sensitivity = keyboardSensitivity;
        }

        doubleTapState = DoubleTapState.NONE;
        multiTouchState = MultiTouchState.NONE;
    }
	
    void Update() {
        CheckDoubleTaps();
        CheckMultiTouches();
        UpdateDeltaX();

        if(Input.touchCount >= 3 && !debugTimer.IsRunning()) {
            debugTimer.StartTimer();
            debugTimer.SetMark(3000);
        }

        if (debugTimer.IsMarkSet() && debugTimer.HasReachedMark() && Input.touchCount >= 3) {
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            } else {
                SceneManager.LoadScene(0);
            }
        }
    }
}
