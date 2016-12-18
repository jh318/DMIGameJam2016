using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIveMeYourINfo : MonoBehaviour {

	public static List<GameObject> presentPool = new List<GameObject>();

	public static GIveMeYourINfo instance;

	void Awake () { // Use this for initialization
		if (instance == null)	instance = this;
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject.tag == "present"){
			StartCoroutine("AddPresent" , other.gameObject);
		}
	}

	IEnumerator AddPresent(GameObject present) { 
		presentPool.Add (present);	
		PresentController pc = present.GetComponent<PresentController>();
		GameManager.instance.AddToWeight(pc.weight);
		GameManager.instance.UpdatePresentCount();
		Debug.Log ("Weightadded");
		yield return new WaitForEndOfFrame();
	}

	public void AddUpPresentValues(){
		foreach (GameObject present in presentPool){
			PresentController pc = present.GetComponent<PresentController>();
			GameManager.instance.AddToScore(pc.scoreValue);
			GameManager.instance.AddToScore(pc.scoreValue);
		}
	}

}
