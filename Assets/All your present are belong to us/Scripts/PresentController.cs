using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentController : MonoBehaviour {

	//Initiate present attributes
	public int scoreValue;
	public int weight;

	// Use this for initialization

	//Check for collision with a 2D object
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			gameObject.SetActive(false);
			//GIveMeYourINfo.instance.AddUpWeight();
	}

}
}
