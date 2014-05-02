using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("GameObject")]
	[System.Serializable]
	public class DestroyComponent : BaseAction {
		public float delay;
		public string component;

		public override void OnEnter ()
		{
			GameObject.Destroy (ownerDefault.GetComponent(component), delay);
			Finish ();
		}
	}
}