using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	private int _score; 	//_score can only be modified using the functions SubtractFromScore and AddToScore.
	public int score{get {return _score;}} 	// Property that references the the private variable _score.
	private int _weight;
	public int weight{get {return _weight;}}

	public Text scoreText;
	public Text presentCount;
	public Text totalWeight;

	void Awake () { // Use this for initialization
		if (instance == null)	instance = this;
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

	public void AddToWeight(int weight){
		instance._weight += weight;
		UpdateTotalWeight ();
		Debug.Log (_weight);
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + _score;
	}

	void UpdatePresentCount()
	{
		presentCount.text = "Presents: " + GIveMeYourINfo.presentPool.Count;
	}

	void UpdateTotalWeight(){
		totalWeight.text = "Weight: " + _weight;
	}
}
