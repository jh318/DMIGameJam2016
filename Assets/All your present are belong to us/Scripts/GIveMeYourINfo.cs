using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIveMeYourINfo : MonoBehaviour {

	public static List<GameObject> presentPool = new List<GameObject>();

	public static GIveMeYourINfo instance;

	void Awake () { // Use this for initialization
		if (instance == null)	instance = this;
	}
	void Update(){

	}

	void OnCollisionEnter (Collision other){
		if (other.gameObject.tag == "present"){
			GameObject present = other.gameObject;
			presentPool.Add (present);
		}
	}

	public void AddUpPresentValues(){
		foreach (GameObject present in presentPool){
			PresentController pc = present.GetComponent<PresentController>();
			Debug.Log ("Here");
			GameManager.instance.AddToScore(pc.scoreValue);
		}
	}
}
