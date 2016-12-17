using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneController : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			//GameObject.FindGameObjectWithTag ("Player").gameObject.GetComponent<GIveMeYourINfo> ().AddUpPresentValues ();
			GIveMeYourINfo.instance.AddUpPresentValues ();

		}
	}
}
