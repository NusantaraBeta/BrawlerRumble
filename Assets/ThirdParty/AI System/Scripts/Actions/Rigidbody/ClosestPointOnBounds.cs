using UnityEngine;
using System.Collections;


namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class ClosestPointOnBounds : BaseAction {
		public Vector3Parameter position;
		[StoreParameter(true,typeof(Vector3Parameter))]
		public string store;

		private UnityEngine.Rigidbody rigidbody;


		public override void OnEnter ()
		{
			rigidbody = ownerDefault.GetComponent<UnityEngine.Rigidbody> ();
			owner.SetVector3 (store, rigidbody.ClosestPointOnBounds (owner.GetValue(position)));
			Finish ();
		}

	}
}
