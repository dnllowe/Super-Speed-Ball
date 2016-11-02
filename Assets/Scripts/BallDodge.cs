using UnityEngine;
using System.Collections;

public class BallDodge : MonoBehaviour {

    /// <summary>
    /// Color of ball (changes transparency during dodge)
    /// </summary>
    Color materialColor;

    /// <summary>
    /// Script for access to ball dash members
    /// </summary>
    BallDash dash;

    /// <summary>
    /// Keeps track of dodge duration
    /// </summary>
    oTimer timer;

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
    /// Color of ball (Vector3)
    /// </summary>
    Color color;

    /// <summary>
    /// Red value of ball
    /// </summary>
    float red;

    /// <summary>
    /// Green value of ball
    /// </summary>
    float green;

    /// <summary>
    /// Blue value of ball
    /// </summary>
    float blue;

    /// <summary>
    /// Alpha value of ball
    /// </summary>
    float alpha;

    /// <summary>
    /// How long dodge phase lasts
    /// </summary>
    public int dodgeDuration = 0;

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
        alpha = dodgeFadeValue;
        color = new Color(red, green, blue, alpha);
        GetComponent<Renderer>().material.color = color;
        timer.RestartTimer();
    }

    //
    void Awake() {
        timer = gameObject.AddComponent<oTimer>();
    }

    // Use this for initialization
    void Start () {
        color = GetComponent<Renderer>().material.color;
        red = color.r;
        green = color.g;
        blue = color.b;
        alpha = color.a;
        dodgeDuration = 500;
        dodgeState = DODGE_STATE.NONE;
        dash = GetComponent<BallDash>();
    }
	
	// Update is called once per frame
	void Update () {
        if (dodgeState == DODGE_STATE.DODGE && timer.GetElapsedTime() >= dodgeDuration) {
            dodgeState = DODGE_STATE.NONE;
            if (onDodgeEnd != null) {
                onDodgeEnd();
            }
            timer.ResetTimer();
            alpha = 1.0f;
            color = new Color(red, green, blue, alpha);
            GetComponent<Renderer>().material.color = color;
        }
    }
}
