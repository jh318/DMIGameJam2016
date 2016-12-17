using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	//_score can only be modified using the functions SubtractFromScore and AddToScore.
	private int _score;
	// Property that references the the private variable _score.
	public int score{
		get {return _score;}
	}

	void Awake () {
		if (instance == null){
			instance = this;
		}
<<<<<<< HEAD
		else{
		}		
=======
>>>>>>> inscreased sluttySanta Move Speed and modified camera for perscetive only.
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	 public void SubtractFromScore(int pointValue) {
        instance._score -= pointValue;
    }

    public void AddToScore(int pointValue) {
        instance._score += pointValue;
		print (score);
    }
}
