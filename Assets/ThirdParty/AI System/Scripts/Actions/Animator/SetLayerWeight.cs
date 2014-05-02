using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class SetLayerWeight : BaseAction {
		public int layer;
		public FloatParameter weight;

		private UnityEngine.Animator animator;

		public override void OnEnter ()
		{
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
			animator.SetLayerWeight (layer, owner.GetValue(weight));
			Finish ();
		}
	}
}