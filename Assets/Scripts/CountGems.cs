using UnityEngine;
using System.Collections;

public class CountGems : MonoBehaviour {

    ScoreKeeper scoreKeeper;

	// Use this for initialization
	void Start () {
        scoreKeeper = GameObject.FindGameObjectWithTag("scoreKeeper").GetComponent<ScoreKeeper>();
        scoreKeeper.IncreaseTotalGemsCount();
	}
}
