using UnityEngine;
using System.Collections;

public class BallDodge : MonoBehaviour {

    /// <summary>
    /// Script for access to ball dash members
    /// </summary>
    BallDash dash;

    /// <summary>
    /// Keeps track of dodge duration
    /// </summary>
    oTimer timer;

    /// <summary>
    /// Color modifier for the ball
    /// </summary>
    ColorModifier color;

    /// <summary>
    /// Keeps track of dodge duration
    /// </summary>
    public oTimer Timer {
        get { return timer; }
    }

    /// <summary>
    /// Enumerator for dodge state. NONE = not dodging. DODGE = in dodge mode. RECHARGE = unable to initiate dodge
    /// </summary>
    enum DODGE_STATE { NONE, DODGE, RECHARGE };

    /// <summary>
    /// Enumerator for dodge state. NONE = not dodging. DODGE = in dodge mode
    /// </summary>
    DODGE_STATE dodgeState;

    /// <summary>
    /// Enumerator for dodge state. NONE = not dodging. DODGE = in dodge mode
    /// </summary>
    DODGE_STATE DodgeState {
        get { return dodgeState; }
    }

    /// <summary>
    /// Delegate for ball entering dodge phase
    /// </summary>
    public delegate void OnDodgeBegan();

    /// <summary>
    /// Event for ball entering dodge phase
    /// </summary>
    public static event OnDodgeBegan onDodgeBegan;

    /// <summary>
    /// Delegate for ball exiting dodge phase
    /// </summary>
    public delegate void OnDodgeEnd();

    /// <summary>
    /// Event for ball exiting dodge phase
    /// </summary>
    public static event OnDodgeEnd onDodgeEnd;

    void OnEnable() {
        PlayerInput.onDoubleTap += Dodge;
    }

    void OnDisable() {
        PlayerInput.onDoubleTap -= Dodge;
    }

    /// <summary>
    /// How long dodge phase lasts
    /// </summary>
    public int dodgeDuration = 500;

    /// <summary>
    /// Opacity level ball fades to during dodge phase
    /// </summary>
    float dodgeFadeValue = 0.25f;

    /// <summary>
    /// Place ball in dodge state. Change direction based on input.DeltaTouch, use fastest vector, and increase by percentage
    /// </summary>
    void Dodge() {

        // Only allow dodge if not in dash state
        if (dash.DashState != BallDash.DASH_STATE.NONE) {
            return;
        }

        dodgeState = DODGE_STATE.DODGE;
        if (onDodgeBegan != null) {
            onDodgeBegan();
        }
        color.SetAlpha(dodgeFadeValue);
        timer.RestartTimer();
    }

    void Awake() {
        timer = gameObject.AddComponent<oTimer>();
    }

    // Use this for initialization
    void Start () {
        dodgeState = DODGE_STATE.NONE;
        dash = GetComponent<BallDash>();
        color = GetComponent<ColorModifier>();
    }
	
	// Update is called once per frame
	void Update () {
        if (dodgeState == DODGE_STATE.DODGE && timer.GetElapsedTime() >= dodgeDuration) {
            dodgeState = DODGE_STATE.NONE;
            if (onDodgeEnd != null) {
                onDodgeEnd();
            }
            timer.ResetTimer();
            color.SetAlpha(1.0f);
        }
    }
}
