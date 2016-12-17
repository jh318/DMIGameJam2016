using UnityEngine;
using System;



public class ModuleList : ScriptableObject
{
	
	public characterModule[] head;
	public characterModule[] outfitUp;
	public characterModule[] outfitDown;
	public characterModule[] bag;

	[Serializable]
	public class characterModule{
		public GameObject parts;
		public Texture2D[] texCol;
	}



}