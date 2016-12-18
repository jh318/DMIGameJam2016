using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	private int _score; 	//_score can only be modified using the functions SubtractFromScore and AddToScore.
	public int score{get {return _score;}} 	// Property that references the the private variable _score.
	public Text scoreText;
	public Text presentCount;
	public int sizeOfPresentPool;

	void Awake () { // Use this for initialization
		if (instance == null)	instance = this;
		sizeOfPresentPool = 0;
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePresentCount ();
	}

	public void SubtractFromScore(int pointValue) {
        instance._score -= pointValue;
		instance.UpdateScore ();
		// AudioManager.PlayEffect("coinLost");
    }

    public void AddToScore(int pointValue) {
        instance._score += pointValue;
		instance.UpdateScore();
		// AudioManager.PlayEffect("coinPickup");
    }

	void UpdateScore ()
	{
		scoreText.text = "Score: " + _score;
	}

	void UpdatePresentCount()
	{
		sizeOfPresentPool = GIveMeYourINfo.presentPool.Count;
		presentCount.text = "Presents: " + sizeOfPresentPool;
	}
}
