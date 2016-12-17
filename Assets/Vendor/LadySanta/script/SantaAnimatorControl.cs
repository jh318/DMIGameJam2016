using UnityEngine;
using System.Collections;

/*
    ChrAnimatorControl is script to control the characters in demoscene.
    will move character , play animation , the position of the weapon , play effect , reaction of key input.
    
	ChrAnimatorControlはデモシーンに配置されたキャラクターを制御するスクリプトです。.
    キャラクターの移動、アニメーションの再生、武器の表示位置、エフェクトの発生、キー入力に対するリアクションを行います。.

    ChrAnimatorControl은 데모신에 배치된 캐릭터를 제어하기위한 스크립트 입니다.
    캐릭터의 이동, 애니메이션의 재생, 무기의 위치, 이펙트의 발생, 키 입력에 대한 반응을 합니다. 
	
	2015.10.24
*/

public class SantaAnimatorControl : MonoBehaviour {

	// required Object or component
	//　制御に必要なオブジェクトなど。
	//　필요한 컴포넨트, 오브젝트 등등.
	public Animator chrAnimator;    // Animator component of character.
	public RuntimeAnimatorController[] chrAnimatorController;// AnimatorController for viewer and interactive
	public CharacterController chrController;    // CharacterController component.
	[Space(20)]
	public SkinnedMeshRenderer mat;	// material.
	public Texture2D[] tex;	// textures.
	private int texIdx;	// Index of tex.
	[Space(20)]
	public GameObject[] items;	// prefab of items.
	private GameObject itemInHand;	// Items that santa girl have.
	public Transform[] itemPoint = new Transform[2]; // attach point (parent object of items)
	public GameObject[] meshData; // character and weapon , the object mesh data is included.

	// to control movement of characters , such as jumps.
	//　キャラクターの移動や、アニメータをコントロールするもの。.
	//　캐릭터의 이동, 점프등을 제어하기 위해서 필요한것.
	[Space(20)]
	public float jumpSpeed = 8.0f;
	public float moveAbilityInAir = 4.0f;
	private float jumpAmount = 0.0f;
	private float runParam = 0.0f;
	private Vector3 moveDirection = Vector3.zero;
	private float gravity = 10.0f;
	private AnimatorStateInfo stateInfo; // Save the state in playing now.

	// power of Through item animation use.
	//　投げるモージョン時に使う投げる力.
	//　아이템을 던질 때의 파워.
	public float[] throughPower = new float[3];
	

	void Update() 
	{
		// Save the state in playing now.
		//　再生中のステートの情報を入れる。.
		// 재생중인 스테이트를 저장.
		stateInfo = chrAnimator.GetCurrentAnimatorStateInfo(0);
		
		// Integer parameter reset to 0. 
		if(!stateInfo.IsTag("InAttack")) 
			chrAnimator.SetInteger("AttackIdx", 0);
		
		// reaction of key input.
		// キー入力に対するリアクションを起こす。.
		// 키입력에 대한 반응.
		// for Attack
		if(Input.GetButtonDown("Fire1") && Input.GetKey("z"))	SetAttack(1);
		else if(Input.GetButtonDown("Fire1") && Input.GetButton("Fire2"))	SetAttack(3);
		else if(Input.GetButtonDown("Fire1"))	SetAttack(2);
		
		// Take out Prezent
		if( Input.GetButtonDown("Fire2") && stateInfo.IsName("na_Idle_00") ){
			chrAnimator.SetBool("Items_Bool", true);
		}
		// for Guard
		if(Input.GetKeyDown("x"))	chrAnimator.SetBool("Guard_Bool", true);
		if(Input.GetKeyUp("x"))	chrAnimator.SetBool("Guard_Bool", false);
		// for Damage
		if(Input.GetKeyDown("c"))	chrAnimator.SetTrigger("Damage_Trg");
		// Failed
		if(Input.GetKeyDown("v")){
			if(stateInfo.IsName("na_Idle_00") || stateInfo.IsName("na_Failed_Loop_00"))
				chrAnimator.SetBool("Failed_Bool", !chrAnimator.GetBool("Failed_Bool") );
		}
		// Success
		if(Input.GetKeyDown("b")){
			if(stateInfo.IsName("na_Idle_00") || stateInfo.IsName("na_Success_Loop_00"))
				chrAnimator.SetBool("Success_Bool", !chrAnimator.GetBool("Success_Bool") );
		}
		
		
		// movement.
		// Input of character moves	
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		Vector3 axisInput = new Vector3(h, 0, v);
		float axisInputMag = axisInput.magnitude;
		if(axisInputMag > 1){
			axisInputMag = 1;
			axisInput.Normalize();
		}

		runParam = 0f;
		if(axisInputMag != 0){
			// for run
			if(Input.GetKey("z"))
				runParam = 1f;
			axisInput = Camera.main.transform.rotation * axisInput;
			axisInput.y = 0;
			// character rotate by scipt
			// free move
			if(axisInput != Vector3.zero)
				transform.forward = axisInput;
		}
		chrAnimator.SetFloat ("Speed", (axisInputMag + runParam));

		// Jump
		// while in jump, I am using Character Controller instead Root Motion, to move the Character.
		// ジャンプ時は、キャラクターコントローラを使ってキャラクターを移動させます。.
		// 점프시에는 캐릭터 컨트롤러를 이용하여 캐릭터를 이동시키고 있습니다.	
		// in ground.
		if(chrController.isGrounded){
			if(stateInfo.IsName("na_Jump_00_fall") || stateInfo.IsName("na_Jump_01_fall") ){
				// jump parameter set to 0.
				chrAnimator.SetInteger("JumpIdx", 0);
				jumpAmount = 0;
			}
			
			if(chrAnimator.GetInteger("JumpIdx") == 0){
				// moveDirection set 0, to prevent to move by Character controller.
				// moveDirectionはゼロにして、キャラクターコントローラがキャラクターを動かさないように。.
				// moveDirection은 0으로 돌려서, 캐릭터 컨트롤러가 캐릭터를 움직이지 않도록한다.
				moveDirection = new Vector3(0, 0, 0);
			}
			
			// press Jump button. make jump
			// if Animator parameter "JumpIdx" is 1, 
			// animator will play state of "Jump_00_start"
			// when play state of "Jump_00_up", animation event will call SetJump()
			// Jumpパラメータからアニメーションが遷移し、.
			// "Jump_00_up"のときにイベントでSetJump()ファンクションを呼ぶ。.
			// Jump파라메터를 통해 스테이트가 점프애니메이션을 재생하고,
			// "Jump_00_up"스테이트를 재생할때 SetJump()를 부른다.
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
			// 空中にいるときはmoveDirectionを使って移動するので、.
			// 方向キーの入力を渡しておく。.
			// 공중에 있는 동안은 캐릭터 컨트롤러를 사용하여 이동하기때문에.
			// 방향키의 입력을 moveDirection에게 전달해준다.
			moveDirection = new Vector3(axisInput.x * moveAbilityInAir, moveDirection.y, axisInput.z * moveAbilityInAir);
			moveDirection.y -= gravity * Time.deltaTime;
			
			// JumpVelocity change the state to while in the air,
			// JumpVelocityは空中でのポーズの制御につかいます。.
			// JumpVelocity는 공중에서의 포즈를 제어합니다.
			chrAnimator.SetFloat("JumpVelocity", (moveDirection.y - (jumpAmount * 0.5f)) );
		}

		// character is move by moveDirection.
		chrController.Move(moveDirection * Time.deltaTime);
	}
	
	// when pressed attack button
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
	// ThroughItem() is called from Animation event.
	// na_ThroughItem, na_ThroughItem_Sp, na_prezentItem
	void ThroughItem(){
		if(itemInHand){
			itemInHand.transform.parent = null;
			itemInHand.GetComponent<Rigidbody>().isKinematic = false;
			int idx = chrAnimator.GetInteger("AttackIdx") - 1;
			// this for viewer mode
			// ビューア用.
			// 뷰어용.
			if(idx < 0){
				idx = 0;
				itemInHand.GetComponent<ItemControl>().waitTime = 1f;
			}
			Vector3 dir = transform.forward * throughPower[idx];
			dir.y = throughPower[idx] * 0.75f;
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

	// change Animator Controller.
	// this function is called from GUIControl.
	// アニメータコントローラを変更する。 .
	// GUIControlスクリプトから呼ばれる。 .
	// ビューアモード、インタラクティブモードが切り替わるときに、各モード用にアニメータコントローラを差し替える。.
	// 애니메이터 컨트롤러를 변경한다.
	// GUIControl 스크립트로부터 불려진다.
	// 뷰어모드, 인터렉티브 모드 사이를 오갈때, 각각의 모드에 맞는 애니메이터를 설정한다.
	public void ControllerChange(int idx){
		if(this.gameObject.activeSelf)
			StartCoroutine (AnimControllerChange (idx));
		else
			chrAnimator.runtimeAnimatorController = chrAnimatorController[idx];
	}
	private IEnumerator AnimControllerChange(int idx){
		// play Idle 0.1 second before change contorller
		// It is prevent error of transform
		PlayClip("na_Idle_00" , 0);
		yield return new WaitForSeconds(0.1f);
		chrAnimator.runtimeAnimatorController = chrAnimatorController[idx];
		PlayClip("na_Idle_00" , 0);
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

	
	
	// read 3D model information.
	// vertex count, triangles, and joint of character and weapon.
	// this function is called from GUIControl.
	public string MeshData(){
		string mdlInfo; // text.
		int[] charData = new int[3]; // vertex.

		charData = GetMeshProperty (meshData);
		mdlInfo = "Character\n      Vertex : " + charData[0].ToString() + ", Tris : " + charData[1].ToString() + ", Bones : " + charData[2].ToString();
		return mdlInfo;
	}
	// collect child of rootObject which have meshrenderer component or skinnedmeshrenderer component.
	GameObject[] CollectMeshRenderer(GameObject rootObject){
		SkinnedMeshRenderer[] skinned = rootObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
		MeshRenderer[] nonSkin = rootObject.GetComponentsInChildren<MeshRenderer> ();
		GameObject[] list;
		if (skinned.Length + nonSkin.Length == 0) {
			list = new GameObject[1];
			list[0] = null;
		}
		else{
			list = new GameObject[skinned.Length + nonSkin.Length];
			for(int i = 0; i < skinned.Length; i++ ){
				list[i] = skinned[i].gameObject;
			}
			for(int i = 0; i < nonSkin.Length; i++ ){
				list[(i + skinned.Length)] = nonSkin[i].gameObject;
			}
		}
		
		return list;
	}
	
	// get vertices, triangles, bone count from gameObject list.
	int[] GetMeshProperty(GameObject[] mesh){
		int[] property = new int[3]; // 0 : vertex, 1 : triangle, 2 : joint.
		Transform[] boneList = null;
		if (mesh [0] != null) {
			for (int i = 0; i < mesh.Length; i++) {
				SkinnedMeshRenderer skinnedMesh = mesh [i].GetComponent<SkinnedMeshRenderer> ();
				// skinned model.
				if (skinnedMesh) {
					property [0] = property [0] + skinnedMesh.sharedMesh.vertices.Length;
					property [1] = property [1] + (skinnedMesh.sharedMesh.triangles.Length / 3);
					if(i == 0){
						boneList = skinnedMesh.bones;
					}
					else{
						boneList = RejectDoubledBones(boneList, skinnedMesh.bones);
					}
					property [2] = boneList.Length;
				}
				// mesh only.
				else {
					property [0] = property [0] + mesh [i].GetComponent<MeshFilter> ().sharedMesh.vertices.Length;
					property [1] = property [1] + (mesh [i].GetComponent<MeshFilter> ().sharedMesh.triangles.Length / 3);
					property [2] = property [2] + 0;
				}
			}
		}
		return property;
	}
	
	// compare bone list and make new bone list that does not overlapped.
	Transform[] RejectDoubledBones(Transform[] boneListA, Transform[] boneListB ){
		Transform[] newList = new Transform[boneListB.Length];
		int idx = 0;
		
		for (int i = 0; i < boneListB.Length; i++) {
			bool check = false;
			for (int j = 0; j < boneListA.Length; j++) {
				if (boneListB[i] == boneListA[j]) {
					check = true;
					break;
				}
			}
			if(!check){
				newList[idx] = boneListB[i];
				idx++;
			}
		}
		
		Transform[] returnList = new Transform[boneListA.Length + idx];
		for (int i = 0; i < boneListA.Length; i++)
			returnList[i] = boneListA[i];
		for (int i = 0; i < idx; i++)
			returnList[i + boneListA.Length] = newList[i];
		
		return returnList;
	}

	// this function is called from GUIControl.
	public void SetShader(int shaderId){
		string[] ShaderName = new string[3];
		ShaderName[0] = "Specular";
		ShaderName[1] = "Diffuse";
		ShaderName[2] = "Unlit/Texture";
		
		for(var i = 0; i < meshData.Length; i++){
			SkinnedMeshRenderer skinnedMeshData = meshData[i].GetComponent<SkinnedMeshRenderer>();
			if(skinnedMeshData){
				skinnedMeshData.material.shader = Shader.Find(ShaderName[shaderId]);
			}
			else{
				meshData[i].GetComponent<MeshRenderer>().material.shader = Shader.Find(ShaderName[shaderId]);
			}
		}
	}
		
	// change Texture
	// this function is called from GUIControl.
	public void ChangeTexture(bool isResetTex){
		if (isResetTex)
			texIdx = 0;
		else {
			texIdx++;
			texIdx = (int)Mathf.Repeat (texIdx, tex.Length);
		}
		mat.material.mainTexture = tex[texIdx];
	}

}
