using UnityEngine;
using System.Collections.Generic;


public class AttackButtonTrigger : MonoBehaviour
{
	static public AttackButtonTrigger current;
	public string NamaGameObject, namaMethodPress, namaMethodRelease;
	private List<EventDelegate> onPress = new List<EventDelegate>();
	private List<EventDelegate> onRelease = new List<EventDelegate>();

	public bool PressEvent, ReleaseEvent;

	void Awake()
	{
		if(PressEvent)
			onPress.Add(new EventDelegate(GameObject.Find(NamaGameObject).GetComponent<PlayerController>(),namaMethodPress));

		if(ReleaseEvent)
			onRelease.Add(new EventDelegate(GameObject.Find(NamaGameObject).GetComponent<PlayerController>(),namaMethodRelease));
	}

	void OnPress (bool pressed)
	{
		current = this;
		if (pressed) EventDelegate.Execute(onPress);
		else EventDelegate.Execute(onRelease);
		current = null;
	}

}
