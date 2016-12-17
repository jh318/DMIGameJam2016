using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour {

	//Initialize movement route variables
	public Vector3 position1;
	public Vector3 position2;
	public float speed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Move object between two positions at declared speed
		transform.position = Vector3.Lerp(
			position1, 
			position2, 
			Mathf.PingPong(
				Time.time*speed, 
				1.0f
			)
		);
	}
}
