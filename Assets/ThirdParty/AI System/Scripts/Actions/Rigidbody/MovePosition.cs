using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class MovePosition : BaseAction {
		public Vector3Parameter position;
		
		private UnityEngine.Rigidbody rigidbody;
		
		public override void OnEnter ()
		{
			rigidbody = ownerDefault.GetComponent<UnityEngine.Rigidbody> ();
		}
		
		public override void OnFixedUpdate()
		{
			rigidbody.MovePosition (owner.GetValue (position));
			Finish ();
		}
	}
}