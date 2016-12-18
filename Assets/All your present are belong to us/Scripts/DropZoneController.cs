using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneController : MonoBehaviour {
	
	private bool isNotTallied = true;
	private GameObject colliderObject;

	void OnTriggerEnter(Collider other){
		StartCoroutine("TallyScore" , other.gameObject);
	}

	IEnumerator TallyScore(GameObject colliderObject) {
		if (colliderObject.tag == "Player" && isNotTallied) {
			colliderObject.gameObject.GetComponent<GIveMeYourINfo> ().AddUpPresentValues ();
			isNotTallied = false;
			Debug.Log ("Here");
			yield return new WaitForEndOfFrame();
		}
	}
}
