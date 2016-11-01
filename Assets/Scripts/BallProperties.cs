﻿using UnityEngine;

public class BallProperties : MonoBehaviour {

    /// <summary>
    /// The panel in the game level
    /// </summary>
    public GameObject panel;

    /// <summary>
    /// Score keeper for game
    /// </summary>
    public ScoreKeeper scoreKeeper;

    // <summary>
    /// Left boundary object for panel movement
    /// </summary>
    public GameObject leftBoundaryObject;

    /// <summary>
    /// Right boundary object for panel movement
    /// </summary>
    public GameObject rightBoundaryObject;

    /// <summary>
    /// The game's current camera
    /// </summary>
    public Camera currentCamera;

    /// <summary>
    /// Starting x vector
    /// </summary>
    public float startX = 0.0f;

    /// <summary>
    /// How much force hits the ball to start each round
    /// </summary>
    public float initialForce = 0.0f;

    /// <summary>
    /// Threshold for change in touch position that registers as different touch location
    /// </summary>
    public float deltaTouchThreshold = 5.0f;
}