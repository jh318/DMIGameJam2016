using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Paraphernalia.Components;
using Paraphernalia.Utils;


public class PlayerController : MonoBehaviour {

    // required Object or component
    public Animator chrAnimator;    // Animator component of character.
    public RuntimeAnimatorController chrAnimatorController;// AnimatorController for viewer and interactive
    public CharacterController chrController;    // CharacterController component.
    [Space(20)]
    public GameObject[] items;  // prefab of items.
    private GameObject itemInHand;  // Items that Santa girl has.
    public Transform[] itemPoint = new Transform[2]; // attach point (parent object of items)
 	
    //HealthController property for triggering hit and fail animations.
    private HealthController _healthCon;
    public HealthController healthCon {get {return _healthCon;}}
    
    private CapsuleCollider collider; // Required for health and damage controllers to work.

    // to control movement of characters , such as jumps.
    [Space(20)]
    public float jumpSpeed = 8.0f;
    public float moveAbilityInAir = 4.0f;
    private float jumpAmount = 0.0f;
    private float runSpeedAdd = 1f;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = 10.0f;
    private AnimatorStateInfo stateInfo; // Save the state in playing now.
    private float runSpeed;
    [Space(20)]
    // power of Throw item animation use.
    public float throwHeightMultiplier = 0.75f;
    public float[] throwPower = new float[3];

    
    void Awake() {
		_healthCon = gameObject.GetComponent<HealthController>();
		collider = gameObject.GetComponent<CapsuleCollider>();
    }
    //Listen for these events while enabled.
    void OnEnable() {
    	_healthCon.onHealthChanged += DamageAnimationTrigger;
    	_healthCon.onDeath += PlayFailAnimation;
    }
    // Stop listening for these events when disabled.
    void OnDisable() {
    	_healthCon.onHealthChanged -= DamageAnimationTrigger;
    	_healthCon.onDeath -= PlayFailAnimation;
    }

    void Update() {
        // Save the state in playing now.
        stateInfo = chrAnimator.GetCurrentAnimatorStateInfo(0);
        
        // Integer parameter reset to 0. 
        if(!stateInfo.IsTag("InAttack")) 
        	chrAnimator.SetInteger("AttackIdx", 0);
        
        // reaction of key input.

        // Take out Present
       	if (Input.GetButtonDown("attack") && chrAnimator.GetBool("Items_Bool") == false) {
        	chrAnimator.SetBool("Items_Bool", true);
    	}
        
        // for Attack
        else if (Input.GetButtonDown("attack")) {
 			SetAttack(2);
	    } 

        if (Input.GetButtonDown("specialAttack") && chrAnimator.GetBool("Items_Bool") == false) {
        	chrAnimator.SetBool("Items_Bool", true);
		}

	 	else if (Input.GetButtonDown("specialAttack")) {
 			SetAttack(3);
	    } 
       
        /*
        // for Guard
        if(Input.GetButtonDown("guard"))   chrAnimator.SetBool("Guard_Bool", true);
        if(Input.GetButtonUp("guard")) chrAnimator.SetBool("Guard_Bool", false);
        // Success
        if(Input.GetKeyDown("b")){
            if(stateInfo.IsName("na_Idle_00") || stateInfo.IsName("na_Success_Loop_00"))
                chrAnimator.SetBool("Success_Bool", !chrAnimator.GetBool("Success_Bool") );
        }
        */
             
        // movement.
        // Input of character moves 
        float h = Input.GetAxis("Horizontal");
        
        Vector3 axisInput = new Vector3(h, 0, 0);
        float axisInputMag = axisInput.magnitude;

        runSpeed = 0f;
        if(axisInputMag != 0){
                runSpeed = 2;
            axisInput = Camera.main.transform.rotation * axisInput;
            axisInput.y = 0;
            // character rotate by script
            // free move
            if(axisInput != Vector3.zero)
                transform.forward = axisInput;
        }
        chrAnimator.SetFloat ("Speed", (axisInputMag * runSpeed));

        // Jump
        // while in jump, I am using Character Controller instead Root Motion, to move the Character.
        // in ground.
        if(chrController.isGrounded){
            if(stateInfo.IsName("na_Jump_00_fall") || stateInfo.IsName("na_Jump_01_fall") ){
                // jump parameter set to 0.
                chrAnimator.SetInteger("JumpIdx", 0);
                jumpAmount = 0;
            }
            
            if(chrAnimator.GetInteger("JumpIdx") == 0){
                // moveDirection set 0, to prevent to move by Character controller.
                moveDirection = Vector3.zero;
            }
            
            // press Jump button. make jump
            // if Animator parameter "JumpIdx" is 1, 
            // animator will play state of "Jump_00_start"
            // when play state of "Jump_00_up", animation event will call SetJump()
            if(Input.GetButtonDown("Jump"))
                chrAnimator.SetInteger("JumpIdx", 1);
        }
        // While in Air
        else if(!chrController.isGrounded){
            // press Jump button. can jump once more.
            if(Input.GetButtonDown("Jump") && chrAnimator.GetInteger("JumpIdx") == 1){
                chrAnimator.SetInteger("JumpIdx", 2);
            }

            // It is moved with Character Controller while in the air,
            // moveDirection is use Axis Input.
            moveDirection = new Vector3(axisInput.x * moveAbilityInAir, moveDirection.y, axisInput.z * moveAbilityInAir);
            moveDirection.y -= gravity * Time.deltaTime;
            
            // JumpVelocity change the state to while in the air,
            chrAnimator.SetFloat("JumpVelocity", (moveDirection.y - (jumpAmount * 0.5f)) );
        }

        // character is move by moveDirection.
        chrController.Move(moveDirection * Time.deltaTime);
    }

    // Delegate of HealthController event onHealthChanged
    void DamageAnimationTrigger(float health, float prevHealth, float maxHealth){
		// for Damage
        if (health < prevHealth){
 			chrAnimator.SetTrigger("Damage_Trg");
		}
		//Debug.Log("Ow! My health is now " + health);
    }
    // Delegate of HealthController event onDeath
    void PlayFailAnimation(){
        chrAnimator.SetBool("Failed_Bool", !chrAnimator.GetBool("Failed_Bool") );
    }

    // control AttackIdx parameter to play attack animation.
    void SetAttack(int param){
        chrAnimator.SetInteger("AttackIdx", param);
    }
    
    
    // Instantiate Items
    // InstanceItem() is called from GUIControl in viewer mode and,
    // Animation event in "na_TakeOutItem_00".
    void InstanceItem(){
        if(!itemInHand){
            int idx = Random.Range(0, items.Length);
            itemInHand = Instantiate(items[idx], itemPoint[0].position, itemPoint[0].rotation) as GameObject;
            itemInHand.transform.parent = itemPoint[0];
            itemInHand.transform.localPosition = Vector3.zero;
            itemInHand.transform.localRotation = Quaternion.identity;
            itemInHand.transform.localScale = Vector3.one;
            itemInHand.GetComponent<ItemControl>().ChangeTextureRandom();
        }
    }
    
    // through Items
    // ThrowItem() is called from Animation event.
    // na_ThrowItem, na_ThrowItem_Sp, na_prezentItem
    void ThroughItem(){
        if(itemInHand){
            itemInHand.transform.parent = null;
            itemInHand.GetComponent<Rigidbody>().isKinematic = false;
            int idx = chrAnimator.GetInteger("AttackIdx") - 1;
            // this for viewer mode
            if(idx < 0){
                idx = 0;
                itemInHand.GetComponent<ItemControl>().waitTime = 1f;
            }
            Vector3 dir = transform.forward * throwPower[idx];
            dir.y = throwPower[idx] * throwHeightMultiplier;
            itemInHand.GetComponent<Rigidbody>().AddForce(dir);
            itemInHand.GetComponent<ItemControl>().InitBullet();
            itemInHand = null;
            chrAnimator.SetBool("Items_Bool", false);
        }
    }
    
    // SetJump() is called from Animation event.
    // Set jumpInput. jumpInput value is used by moveDirection.y in next Update() . 
    void SetJump(){
        if (chrAnimator.GetInteger ("JumpIdx") <= 1) {
            moveDirection = new Vector3(0, jumpSpeed, 0);
            // when in ground.
            jumpAmount += jumpSpeed;
            chrAnimator.SetInteger ("JumpIdx", 1);
            chrAnimator.SetFloat("JumpVelocity", jumpAmount * 0.5f );
        }
        else if (chrAnimator.GetInteger ("JumpIdx") == 2) {
            // jump in air
            moveDirection.y += jumpSpeed * 1.5f;
            jumpAmount += jumpSpeed * 1.5f;
            chrAnimator.SetInteger ("JumpIdx", 3);
        }
    }

    // play animation state.
    // for viewer mode
    public void PlayClip(string stateName , int item){
        if(item == 0 && itemInHand){
            itemInHand.GetComponent<ItemControl>().DestroyItem(0f);
        }
        else if(item == 1 && !itemInHand){
            InstanceItem();
        }
        chrAnimator.CrossFade(stateName, 0.05f);
    }

}
