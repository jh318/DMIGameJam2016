using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIveMeYourINfo : MonoBehaviour {

	public static List<GameObject> presentPool = new List<GameObject>();

	private bool hasReported = false;

	void Update(){
        if(presentPool.Count >= 4 && !hasReported)   AddUpPresentValues();

	}

	void OnCollisionEnter (Collision other){
		if (other.gameObject.tag == "present"){
			GameObject present = other.gameObject;
			presentPool.Add (present);
		}
	}

	void AddUpPresentValues(){
		foreach (GameObject present in presentPool){
			PresentController pc = present.GetComponent<PresentController>();
			GameManager.instance.AddToScore(pc.scoreValue);
		}
		hasReported = true;
	}
}
