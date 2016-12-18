using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject player;

	private int _score; 	//_score can only be modified using the functions SubtractFromScore and AddToScore.
	public int score{get {return _score;}} 	// Property that references the the private variable _score.
	private int _weight;
	public int weight{get {return _weight;}}

	public int presentCount = 3;

	public Text scoreText;
	public Text presentCountText;
	public Text healthText;
	private float health;

	void Awake () { // Use this for initialization
		if (instance == null)	instance = this;
	}

	void Update(){
		health = player.GetComponent<HealthController>().health;
		healthText.text = "Health: " + health;
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
		healthText.text = "Health: " + health;
	}

	public void AddPresentToCount(int count){
		instance.presentCount += count;
		presentCountText.text = "Presents: " + presentCount;
	}

	public void PresentThown(int numThrown){
		// instance.presentCount -= numThrown;
		// presentCountText.text = "Presents: " + presentCount;
		GIveMeYourINfo.instance.RemovePresent(numThrown);
		UpdatePresentCount();
	}

	public void UpdatePresentCount() // List method
	{	
		instance.presentCount = GIveMeYourINfo.presentPool.Count;
		presentCountText.text = "Presents: " + presentCount;
	}


	void UpdateTotalWeight(){
		// totalWeight.text = "Weight: " + _weight;
	}
}
