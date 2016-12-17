using UnityEngine;
using System.Collections;


/*
    ModuleSetupControl is script for combine and change module prefab to one character.

    ModuleSetupControlはキャラクターパーツを調合、変更するためのスクリプトです。

    ModuleSetupControl은 캐릭터 모듈을 조합, 변경하기위한 스크립트입니다.
	
	2016.10.10
*/



public class ModuleSetupControl : MonoBehaviour {

	private SantaAnimatorControl chrCtrl;

	[SerializeField]
	private Transform rootBone;
	private Transform[] boneStructure;

	[SerializeField]
	public ModuleList modules;

	private SkinnedMeshRenderer[] current_head;
	private Texture2D[] current_head_tex;
	private SkinnedMeshRenderer[] current_outfitUp;
	private Texture2D[] current_outfitUp_tex;
	private SkinnedMeshRenderer[] current_outfitDown;
	private Texture2D[] current_outfitDown_tex;
	private SkinnedMeshRenderer[] current_bag;
	private Texture2D[] current_bag_tex;


	// Use this for initialization
	void Awake () {
		chrCtrl = this.GetComponent<SantaAnimatorControl> ();
		boneStructure = rootBone.GetComponentsInChildren<Transform> ();
		// SetDefaultModule ();
	}


	SkinnedMeshRenderer[] ModuleSetter (GameObject module) {
		GameObject m = Instantiate (module);
		SkinnedMeshRenderer[] mMesh = m.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < mMesh.Length; i++) {
			mMesh [i].transform.parent = this.transform;
			mMesh [i].transform.localPosition = Vector3.zero;
			mMesh [i].transform.localRotation = Quaternion.identity;
			mMesh [i].transform.localScale = Vector3.one;

			for (int k = 0; k < boneStructure.Length; k++) {
				if (mMesh [i].rootBone.name == boneStructure [k].name) {
					mMesh [i].rootBone = boneStructure [k];
					break;
				}
			}

			Transform[] newBones = new Transform[mMesh[i].bones.Length];
			int idx = 0;
			for (int j = 0; j < mMesh[i].bones.Length; j++) {
				for (int k = 0; k < boneStructure.Length; k++) {
					if (mMesh [i].bones [j].name == boneStructure [k].name) {
						newBones [idx] = boneStructure [k];
						idx++;
						break;
					}
				}
			}
			mMesh [i].bones = newBones;
		}

		Destroy (m);
		return mMesh;
	}


	public void ModuleRemove (SkinnedMeshRenderer[] currentModule) {
		if (currentModule.Length != 0) {
			for (int i = 0; i < currentModule.Length; i++) {
				Destroy (currentModule [i].gameObject);
			}
		}
	}

	public void ModuleSelector (int parts, int partsID) {
		switch (parts) {
		case 0: 
			if (current_head != null) {
				ModuleRemove (current_head);
			}
			current_head = ModuleSetter (modules.head[partsID].parts);
			current_head_tex = modules.head[partsID].texCol;
			break;
		case 1: 
			if (current_outfitUp != null) {
				ModuleRemove (current_outfitUp);
			}
			current_outfitUp = ModuleSetter (modules.outfitUp[partsID].parts);
			current_outfitUp_tex = modules.outfitUp[partsID].texCol;
			break;
		case 2: 	
			if (current_outfitDown != null) {
				ModuleRemove (current_outfitDown);
			}
			current_outfitDown = ModuleSetter (modules.outfitDown[partsID].parts);
			current_outfitDown_tex = modules.outfitDown[partsID].texCol;
			break;
		case 3: 
			if (current_bag != null) {
				ModuleRemove (current_bag);
			}
			current_bag = ModuleSetter (modules.bag[partsID].parts);
			current_bag_tex = modules.bag[partsID].texCol;
			break;
		}
	}


	public void SetAllModule (int headId, int outfitUpId, int outfitDownId, int bagId) {
		ModuleSelector (0, headId);
		ModuleSelector (1, outfitUpId);
		ModuleSelector (2, outfitDownId);
		ModuleSelector (3, bagId);
	}


	public void ColorSetter (SkinnedMeshRenderer[] currentModule, Texture2D tex) {
		if (currentModule.Length != 0) {
			for (int i = 0; i < currentModule.Length; i++) {
				currentModule [i].material.mainTexture = tex;
			}
		}
	}

	public void ColorSelector (int parts, int colorID) {
		switch (parts) {
		case 0: 
			if (current_head != null)
				ColorSetter (current_head, current_head_tex[colorID]);
			break;
		case 1: 
			if (current_outfitUp != null)
				ColorSetter (current_outfitUp, current_outfitUp_tex[colorID]);
			break;
		case 2: 	
			if (current_outfitDown != null)
				ColorSetter (current_outfitDown, current_outfitDown_tex[colorID]);
			break;
		case 3: 
			if (current_bag != null)
				ColorSetter (current_bag, current_bag_tex[colorID]);
			break;
		}
	}

	public void SetAllColor (int headId, int outfitUpId, int outfitDownId, int bagId) {
		ColorSelector (0, headId);
		ColorSelector (1, outfitUpId);
		ColorSelector (2, outfitDownId);
		ColorSelector (3, bagId);
	}



	public void UpdateMeshData () {
		int idx = 0;
		chrCtrl.meshData = new GameObject[(current_head.Length + current_outfitUp.Length + current_outfitDown.Length + current_bag.Length)];

		for (int i = 0; i < current_head.Length; i++) {
			chrCtrl.meshData [idx] = current_head [i].gameObject;
			idx++;
		}
		for (int i = 0; i < current_outfitUp.Length; i++) {
			chrCtrl.meshData [idx] = current_outfitUp [i].gameObject;
			idx++;
		}
		for (int i = 0; i < current_outfitDown.Length; i++) {
			chrCtrl.meshData [idx] = current_outfitDown [i].gameObject;
			idx++;
		}
		for (int i = 0; i < current_bag.Length; i++) {
			chrCtrl.meshData [idx] = current_bag [i].gameObject;
			idx++;
		}
	}

}
