using System.Collections;
using System.Collections.Generic;
using System;

public class Present : IComparable<Present> {
	//Private
	private string tag;
	private int scoreValue;
	private int power;
	//private int weight
	//Public
	//Constructor?
	public Present(string tag, int newScoreValue) 
	{
		scoreValue = newScoreValue;
	}
	public int CompareTo(Present other)
	{
		if (other == null) {
			return 1;
		}
		return power - other.power;
	}
	int getScoreValue(){ return scoreValue;}

}
