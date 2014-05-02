using UnityEngine;
using System.Collections;

public class MinMaxAttribute : PropertyAttribute {
	public float minLimit;
	public float maxLimit;
	public bool roundToInt;

	public MinMaxAttribute(float minLimit, float maxLimit)
	{
		this.minLimit = minLimit;
		this.maxLimit = maxLimit;
	}

	public MinMaxAttribute(float minLimit, float maxLimit,bool roundToInt)
	{
		this.minLimit = minLimit;
		this.maxLimit = maxLimit;
		this.roundToInt = roundToInt;
	}
}
