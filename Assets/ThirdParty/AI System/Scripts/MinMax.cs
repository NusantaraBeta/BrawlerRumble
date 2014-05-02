using UnityEngine;
using System.Collections;

[System.Serializable]
public class MinMax {
	public float min;
	public float max;

	public float GetRandom(){
		return Random.Range(min,max);
	}
}
