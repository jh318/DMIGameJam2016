using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropZoneController : MonoBehaviour {
	
	private bool isNotTallied = true;
	private GameObject colliderObject;
	public GameObject WinText;
	public Text winText;
	private bool gameEnd = false;

	void OnTriggerEnter(Collider other){
		StartCoroutine("TallyScore" , other.gameObject);
		if (other.tag == "Player") {
			winText.text = "You Win!\n Final Score: " + GameManager.instance.score + "\nPress any key to \nrestart.";
			WinText.SetActive(true);
			gameEnd = true;
			}
		}
	void OnTriggerStay(){
		if (gameEnd == true && Input.anyKeyDown) {
			Application.LoadLevel (0);
		}
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
