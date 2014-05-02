using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class AddForceAtPosition : BaseAction {
		public ForceMode forceMode;
		public Vector3Parameter force;
		public Vector3Parameter position;
		
		private UnityEngine.Rigidbody rigidbody;
		
		public override void OnEnter ()
		{
			rigidbody = ownerDefault.GetComponent<UnityEngine.Rigidbody> ();
		}
		
		public override void OnFixedUpdate()
		{
			if (rigidbody != null) {
				rigidbody.AddForceAtPosition (owner.GetValue(force),owner.GetValue(position), forceMode);
			}
			Finish ();
		}
	}
}