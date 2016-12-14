using UnityEngine;
using System.Collections;

/// <summary>
/// Activates / Deactivates gems on collision with ball
/// </summary>
public class BallCollision : MonoBehaviour {

    /// <summary>
    /// Keeps track of game scoring. Used here to reset multiplier when ball hits panel
    /// </summary>
    ScoreKeeper scoreKeeper;

	void OnTriggerEnter(Collider other) {

		if(other.CompareTag("gem")) {
			other.gameObject.SetActive(false);
            scoreKeeper.IncreaseGemsCollected();
            scoreKeeper.IncreaseConsecutiveGemsCollected();
            scoreKeeper.IncreaseScore();
            scoreKeeper.IncreaseMultiplier();
            scoreKeeper.UpdateScore();

            if(scoreKeeper.GemsCollected >= scoreKeeper.TotalGems) {
                scoreKeeper.IsGameWon = true;
                scoreKeeper.GetTransitionTimer().StartTimer();
                scoreKeeper.GetTransitionTimer().SetMark(scoreKeeper.RestartTime);
            } 
		} 
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("panelBottom") || 
            collision.gameObject.CompareTag("panelTop")) {
            scoreKeeper.ResetMultiplier();
            scoreKeeper.ResetConsecutiveGemsCollected();
        }
    }

    void Start() {
        scoreKeeper = GameObject.FindGameObjectWithTag("scoreKeeper").GetComponent<ScoreKeeper>();
    }
}
