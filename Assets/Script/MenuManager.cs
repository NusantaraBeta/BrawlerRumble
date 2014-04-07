using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	public Transform Player;
	public Transform playerStartPos;

	void Awake()
	{
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		Player.position = playerStartPos.position;
		Player.rotation = playerStartPos.rotation;
	}

	public void loadGame()
	{
		Application.LoadLevel("Battle");
	}

	public void LoadHome()
	{
		Application.LoadLevel("Home");
	}
}
