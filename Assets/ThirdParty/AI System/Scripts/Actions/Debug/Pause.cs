using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Debug")]
	[System.Serializable]
	public class Pause : BaseAction {
		public bool state=true;
		public override void OnEnter ()
		{
			#if UNITY_EDITOR
			EditorApplication.isPaused=state;
			#endif
			Finish ();
		}
	}
}