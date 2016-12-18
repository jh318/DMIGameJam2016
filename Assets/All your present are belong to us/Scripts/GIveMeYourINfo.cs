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
			//GameManager.instance.AddPresentToCount(1);
		}
	}

	IEnumerator AddPresent(GameObject present) {
		presentPool.Add (present);	
		PresentController pc = present.GetComponent<PresentController>();
		GameManager.instance.AddToWeight(pc.weight);
		GameManager.instance.UpdatePresentCount();
		yield return new WaitForEndOfFrame();
	}

	public void RemovePresent(int numRemoved){
		for (int i = 1; i <= numRemoved; i++){
			presentPool.RemoveAt(0);
		}
	}

	public void AddUpPresentValues(){
		foreach (GameObject present in presentPool){
			PresentController pc = present.GetComponent<PresentController>();
			GameManager.instance.AddToScore(pc.scoreValue);
		}
	}

}
