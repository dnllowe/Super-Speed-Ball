using UnityEngine;
using System.Collections;

public class CountGems : MonoBehaviour {

    /// <summary>
    /// Keeps track of game scoring. Used here so score is aware of total gems in level
    /// </summary>
    ScoreKeeper scoreKeeper;

	// Use this for initialization
	void Start () {
        scoreKeeper = GameObject.FindGameObjectWithTag("scoreKeeper").GetComponent<ScoreKeeper>();
        scoreKeeper.IncreaseTotalGemsCount();
	}
}
