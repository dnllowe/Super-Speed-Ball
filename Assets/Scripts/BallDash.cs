using UnityEngine;
using System.Collections;

public class BallDash : MonoBehaviour {

    /// <summary>
    /// Script for access to ball movement methods
    /// </summary>
    BallMovement ballMovement;

    /// <summary>
    /// Collider for ball object
    /// </summary>
    Collider ballCollider;

    /// <summary>
    /// Physics body for collision and velocity calculations
    /// </summary>
    Rigidbody rigidBody;

    /// <summary>
    /// Color modifier for ball
    /// </summary>
    ColorModifier color;

    /// <summary>
    /// Track player inputs
    /// </summary>
    PlayerInput input;

    /// <summary>
    /// Where player cursor or touch is positioned relative to world space for Dash
    /// </summary>
    Vector3 aim;

    /// <summary>
    /// Last recorded velocity before dash state change
    /// </summary>
    Vector3 previousVelocity;

    /// <summary>
    /// Position of the ball when dash began
    /// </summary>
    Vector3 preDashPosition;

    /// <summary>
    /// How long dash mode lasts
    /// </summary>
    public int dashDuration = 1000;

    /// <summary>
    /// Opacity level ball fades to during dodge phase
    /// </summary>
    float dashFadeValue = 0.25f;

    /// <summary>
    /// Enumerator for dash state. 
    /// NONE = not dashing. 
    /// CHARGED = ready to initiate. 
    /// BEGAN = ball frozen, but not aiming. 
    /// AIM = player is aiming new direction
    /// DASH = ball is in dash state for specified time
    /// </summary>
    public enum DASH_STATE { NONE, CHARGED, BEGAN, AIM, DASH };

    /// <summary>
    /// Enumerator for dash state. 
    /// NONE = not dashing. 
    /// CHARGED = ready to initiate. 
    /// BEGAN = ball frozen, but not aiming. 
    /// AIM = player is aiming new direction
    /// DASH = ball is in dash state for specified time
    /// </summary>
    DASH_STATE dashState;

    /// <summary>
    /// Enumerator for dash state. 
    /// NONE = not dashing. 
    /// CHARGED = ready to initiate. 
    /// BEGAN = ball frozen, but not aiming. 
    /// AIM = player is aiming new direction
    /// DASH = ball is in dash state for specified time
    /// </summary>
    public DASH_STATE DashState {
        get { return dashState; }
    }

    /// <summary>
    /// Timer for dodge script
    /// </summary>
    oTimer dodgeTimer;

    void OnEnable() {

        PlayerInput.onMultiTouchBegan += UpdateDashState;
        PlayerInput.onMultiToSingle += UpdateDashState;
        PlayerInput.onMultiTouchEnd += UpdateDashState;
        PlayerInput.onDoubleSimultaneousTouch += DashEnter;
        PlayerInput.onDoubleSimultaneousToSingle += DashAimBegan;
    }

    void OnDisable() {
        PlayerInput.onMultiTouchBegan -= UpdateDashState;
        PlayerInput.onMultiToSingle -= UpdateDashState;
        PlayerInput.onMultiTouchEnd -= UpdateDashState;
        PlayerInput.onDoubleSimultaneousTouch -= DashEnter;
        PlayerInput.onDoubleSimultaneousToSingle -= DashAimBegan;
    }

    /// <summary>
    /// Updates dash state based on changing player input
    /// </summary>
    void UpdateDashState() {

        // Player went from aim phase (using simultaneous to single), back to began, and now back to dash aim
        if(dashState == DASH_STATE.BEGAN && 
            input.MultiState == PlayerInput.MultiTouchState.TO_SINGLE &&
            input.SimultaneousState != PlayerInput.SimultaneousTouchState.DOUBLE_DETECTED) {
            DashAimBegan();
        }

        // Player goes from aim with single touch back to multi touch
        if (dashState == DASH_STATE.AIM && 
            input.MultiState == PlayerInput.MultiTouchState.MULTI) {
            DashEnter();
        }
/*
        // Cancel dash if player releases multitouch without entering aim phase
        if(dashState == DASH_STATE.BEGAN &&
           input.MultiState == PlayerInput.MultiTouchState.NONE) {
            DashCancel();
        }
*/
    }

    /// <summary>
    /// Stops ball and turns off collision detection (by entering dodge mode)
    /// </summary>
    void DashEnter() {
        ballMovement.UpdateMaxSpeed();
        ballCollider.enabled = false;
        if (dashState == DASH_STATE.NONE || dashState == DASH_STATE.DASH) {
            previousVelocity = rigidBody.velocity;
            color.SetAlpha(dashFadeValue);
            preDashPosition = transform.position;
        }

        rigidBody.velocity = new Vector3(0, 0, 0);
        dashState = DASH_STATE.BEGAN;

        // Pause timer to delay dodging until dash begins
        dodgeTimer.PauseTimer();
    }

    /// <summary>
    /// Enter aiming phase of dash. 
    /// </summary>
    void DashAimBegan() {
        dashState = DASH_STATE.AIM;
    }

    /// <summary>
    /// Player controls direction ball will shoot when released
    /// </summary>
    void DashAim() {
        if (SystemInfo.deviceType == DeviceType.Handheld) {

            if (Input.touchCount > 0) {
                // There can only be one touch finger during aim state
                var touch = Input.GetTouch(0);

                // Convert ray to vector at plane that intersects object (ball)
                aim = oFunctions.ConvertTouchToGameCoordinates(touch, gameObject);

                // Perform dash once player releases finger
                if (touch.phase == TouchPhase.Ended) {
                    Dash();
                }
            }

        } else if (SystemInfo.deviceType == DeviceType.Desktop) {
            var mouse = Input.mousePosition;

            // Convert ray to vector at plane that intersects object (ball)
            aim = oFunctions.ConvertMouseToGameCoordinates(mouse, gameObject);
        }
    }

    /// <summary>
    /// Release ball in new direction (set by player). Resume previous velocity
    /// </summary>
    void Dash() {

        dashState = DASH_STATE.DASH;
        ballCollider.enabled = true;
        transform.position = preDashPosition;

        if (SystemInfo.deviceType == DeviceType.Handheld) {
            // Flip the output to aim opposite or where touch / mouse are pointed
            var newPosition = new Vector3(aim.x, aim.y, 0);
            var newDirection = transform.position - newPosition;

            // Need normalized deltaTouch to determine new direction without affecting speed
            newDirection.Normalize();
            ballMovement.IncreaseMaxSpeed();
            var newVelocity = new Vector3(ballMovement.MaxSpeed * newDirection.x,
                ballMovement.MaxSpeed * newDirection.y, 0.0f);
            rigidBody.velocity = newVelocity;

        } else if (SystemInfo.deviceType == DeviceType.Desktop) {
            // Flip the output to aim opposite or where touch / mouse are pointed
            var newPosition = new Vector3(aim.x, aim.y, 0);
            var newDirection = transform.position - newPosition;

            // Need normalized deltaTouch to determine new direction without affecting speed
            newDirection.Normalize();
            ballMovement.IncreaseMaxSpeed();
            var newVelocity = new Vector3(ballMovement.MaxSpeed * newDirection.x,
                ballMovement.MaxSpeed * newDirection.y, 0.0f);
            rigidBody.velocity = newVelocity;
        }

        color.SetAlpha(1.0f);

        // Unpause timer to resume dodging
        dodgeTimer.UnpauseTimer();
    }

    /// <summary>
    /// Cancel dash state without dashing. Put previous ball properties back.
    /// </summary>
    void DashCancel() {

        transform.position = preDashPosition;
        rigidBody.velocity = previousVelocity;
        ballCollider.enabled = true;
        color.SetAlpha(1.0f);

        // Unpause timer to resume dodging
        dodgeTimer.UnpauseTimer();
    }

    // Use this for initialization
    void Start () {
        ballCollider = GetComponent<Collider>();
        rigidBody = GetComponent<Rigidbody>();
        dodgeTimer = GetComponent<BallDodge>().Timer;
        ballMovement = GetComponent<BallMovement>();
        color = GetComponent<ColorModifier>();
        input = GameObject.FindGameObjectWithTag("input").GetComponent<PlayerInput>();
        dashState = DASH_STATE.NONE;
    }

    // Update is called once per frame
    void Update() {
        if (dashState == DASH_STATE.AIM) {
            DashAim();
        }

        // Do not keep max speed if in dash state
        if (dashState == DASH_STATE.NONE) {
            ballMovement.UpdateMaxSpeed();
            ballMovement.MaintainMaxSpeed();
        }
    }
}
