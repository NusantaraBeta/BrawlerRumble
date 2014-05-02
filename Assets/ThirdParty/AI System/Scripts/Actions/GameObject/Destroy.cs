using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("GameObject")]
	[System.Serializable]
	public class Destroy : BaseAction {
		public float delay;

		public override void OnEnter ()
		{
			GameObject.Destroy (ownerDefault, delay);
			Finish ();
		}
	}
}