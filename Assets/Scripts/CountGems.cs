using UnityEngine;
using System.Collections;

public class CountGems : MonoBehaviour {

    public ScoreKeeper scoreKeeper;

	// Use this for initialization
	void Start () {
        scoreKeeper.IncreaseTotalGemsCount();
	}
}
