using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour {

	//Initialize movement route variables
	// [ToolTip("This is measured in meters and will be measured from the startingPos of the enemy")]
	public float distanceRight = 0.0f;
	// [ToolTip("This is measured in meters and will be measured from the startingPos of the enemy")]
	public float distanceLeft = 0.0f;
	public float speed = 1.0f;

	private Vector3 positionLeft;
	private Vector3 positionRight;
	private Vector3 startingPos;
	// Use this for initialization
	void OnEnable () {
		startingPos = gameObject.transform.position;
		positionLeft = new Vector3((startingPos.x + distanceLeft), startingPos.y, startingPos.z);
		positionRight = new Vector3((startingPos.x - distanceRight), startingPos.y, startingPos.z);

	}
	
	// Update is called once per frame
	void Update () {
		//Move object between two positions at declared speed
		transform.position = Vector3.Lerp(
			positionLeft, 
			positionRight, 
			Mathf.PingPong(
				Time.time*speed, 
				1.0f
			)
		);
	}
}
