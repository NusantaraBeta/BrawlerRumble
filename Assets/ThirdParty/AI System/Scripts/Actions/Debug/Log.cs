using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Debug")]
	[System.Serializable]
	public class Log : BaseAction {
		public string message;
		public override void OnEnter ()
		{
			Debug.Log (message);
			Finish ();
		}
	}
}