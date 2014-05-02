using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class AddExplosionForce : BaseAction {
		public FloatParameter force;
		public FloatParameter radius;
		public FloatParameter upwardsModifier;
		public ForceMode forceMode;
		public Vector3Parameter position;

		private UnityEngine.Rigidbody rigidbody;
		
		public override void OnEnter ()
		{
			rigidbody = ownerDefault.GetComponent<UnityEngine.Rigidbody> ();
		}
		
		public override void OnFixedUpdate()
		{
			if (rigidbody != null) {
				rigidbody.AddExplosionForce (owner.GetValue(force),owner.GetValue(position),owner.GetValue(radius),owner.GetValue(upwardsModifier), forceMode);
			}
			Finish ();
		}
	}
}