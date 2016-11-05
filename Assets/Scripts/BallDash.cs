﻿using UnityEngine;
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
    /// Enumerator for dash state. NONE = not dashing. CHARGED = ready to initiate. BEGAN = ball frozen, but not aiming. AIM = player is aiming new direction
    /// </summary>
    public enum DASH_STATE { NONE, CHARGED, BEGAN, AIM };

    /// <summary>
    /// Enumerator for dash state. NONE = not dashing. BEGAN = ball frozen, but not aiming. AIM = player is aiming new direction
    /// </summary>
    DASH_STATE dashState;

    /// <summary>
    /// Enumerator for dash state. NONE = not dashing. BEGAN = ball frozen, but not aiming. AIM = player is aiming new direction
    /// </summary>
    public DASH_STATE DashState {
        get { return dashState; }
    }

    /// <summary>
    /// Timer for dodge script
    /// </summary>
    oTimer dodgeTimer;

    void OnEnable() {
        
        PlayerInput.onMultiTouchBegan += DashEnter;
        PlayerInput.onMultiToSingle += DashAimBegan;
        PlayerInput.onMultiTouchEnd += Dash; // Program Dash() so that it only works if in AIM state (to avoid quick press and release from ball in random direction)
    }

    void OnDisable() {
        PlayerInput.onMultiTouchBegan -= DashEnter;
        PlayerInput.onMultiToSingle -= DashAimBegan;
        PlayerInput.onMultiTouchEnd -= Dash;
    }

    /// <summary>
    /// Stops ball and turns off collision detection (by entering dodge mode)
    /// </summary>
    void DashEnter() {
        ballMovement.UpdateMaxSpeed();
        ballCollider.enabled = false;
        if (dashState == DASH_STATE.NONE) {
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
            var touch = Input.touches[Input.touchCount - 1];

            // Convert ray to vector at plane that intersects object (ball)
            aim = oFunctions.ConvertTouchToGameCoordinates(touch, gameObject);

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
        ballCollider.enabled = true;
        transform.position = preDashPosition;

        // RETURNS HERE
        if (dashState != DASH_STATE.AIM) {
            rigidBody.velocity = previousVelocity;
            dashState = DASH_STATE.NONE;
            color.SetAlpha(1.0f);

            // Unpause timer to resume dodging
            dodgeTimer.UnpauseTimer();

            return;
        }

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
        dashState = DASH_STATE.NONE;
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
