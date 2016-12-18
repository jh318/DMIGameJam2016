using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Paraphernalia.Extensions;

public class BasicEnemyMovement : MonoBehaviour {

	//Initialize movement route variables
	// [ToolTip("This is measured in meters and will be measured from the startingPos of the enemy")]
	public float distanceRight = 0.0f;
	// [ToolTip("This is measured in meters and will be measured from the startingPos of the enemy")]
	public float distanceLeft = 0.0f;
	public float speed = 1.0f;

	private CapsuleCollider collider;
	private HealthController _healthCon;
	private Rigidbody body;
	private Animator animator;

	private bool isDisabled = false;
	private Vector3 positionLeft;
	private Vector3 positionRight;
	private Vector3 startingPos;

	// Use this for initialization
    void Awake() {
		_healthCon = gameObject.GetComponent<HealthController>();
		collider = gameObject.GetComponent<CapsuleCollider>();
		animator = gameObject.GetComponent<Animator>();
		body = gameObject.GetComponent<Rigidbody>();
    }

	void OnEnable () {
		startingPos = gameObject.transform.position;
		positionLeft = new Vector3((startingPos.x + distanceLeft), startingPos.y, startingPos.z);
		positionRight = new Vector3((startingPos.x - distanceRight), startingPos.y, startingPos.z);
    
    //Listen for these events while enabled.
    	_healthCon.onDeath += DisableMovementOnDeath;
    	_healthCon.onHealthChanged += PlayHitAnimation;
    }

    // Stop listening for these events when disabled.
    void OnDisable() {
    	_healthCon.onDeath -= DisableMovementOnDeath;
    	// _healthCon.onDestruction -= SpawnDeathParticles;
    }
	
	// Update is called once per frame
	void Update () {
		//Move object between two positions at declared speed
		if (isDisabled ==  false){
			Move();
		}
	}

	void DisableMovementOnDeath(){
		// StartCoroutine("DisableMovementOnDeathCoroutine");
		isDisabled = true;
		animator.SetBool("isDisabled", true);
	}
		
	void PlayHitAnimation(float health, float prevHealth, float maxHealth){
		if (health < prevHealth){
			animator.SetTrigger("isHit");		
		}
	}

	public void SpawnDeathParticles(){
		Spawner.Spawn("SnowmanDeath");
	}

	void Move(){
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
