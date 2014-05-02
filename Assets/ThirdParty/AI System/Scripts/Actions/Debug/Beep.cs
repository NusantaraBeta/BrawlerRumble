using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Debug")]
	[System.Serializable]
	public class Beep : BaseAction {
		public override void OnEnter ()
		{
			#if UNITY_EDITOR
			EditorApplication.Beep();
			#endif
			Finish ();
		}
	}
}