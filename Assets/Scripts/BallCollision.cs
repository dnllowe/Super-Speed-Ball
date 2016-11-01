﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Activates / Deactivates gems on collision with ball
/// </summary>
public class BallCollision : MonoBehaviour {

    ScoreKeeper scoreKeeper;

	void OnTriggerEnter(Collider other) {

		if(other.CompareTag("gem")) {
			other.gameObject.SetActive(false);
            scoreKeeper.IncreaseGemsCollected();
            scoreKeeper.IncreaseConsecutiveGemsCollected();
            scoreKeeper.IncreaseScore();
            scoreKeeper.IncreaseMultiplier(); 
            
            if(scoreKeeper.GemsCollected >= scoreKeeper.TotalGems) {
                scoreKeeper.IsGameWon = true;
                scoreKeeper.GetTimer().StartTimer();
                scoreKeeper.GetTimer().SetMark(scoreKeeper.RestartTime);
            } 
		} 
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("panel")) {
            scoreKeeper.ResetMultiplier();
            scoreKeeper.ResetConsecutiveGemsCollected();
        }
    }

    void Start() {
        scoreKeeper = GetComponent<BallProperties>().scoreKeeper;
    }
}