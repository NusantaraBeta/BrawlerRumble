using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {
	public Transform Player;
	public Transform playerStartPos;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		Player.position = playerStartPos.position;
	}
	
	public void backToHome()
	{
		Application.LoadLevel("Home");
	}
}
