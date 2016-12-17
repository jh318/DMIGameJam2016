using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentController : MonoBehaviour {

	//Initiate present attributes
	public int scoreValue;
	public string objectTag; // Object to collides with. Set to player.





	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
		/**if (Collision.collider.tag == collideWithThisTag) 
		{
			
		}*/
	}

	//Check for collision with a 2D object
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag==objectTag)
			Destroy(gameObject);
			GameManager.instance.AddToScore(score);
		//TODO add to player score
		//ScoreController.score
		//GameManager.GetComponent<GameManager>().AddToScore(scoreValue);
		GameManager.instance.AddToScore(scoreValue);
	}

}
