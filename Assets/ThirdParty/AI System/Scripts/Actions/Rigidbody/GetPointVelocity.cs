using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class GetPointVelocity : BaseAction {
		[StoreParameter(true,typeof(Vector3Parameter))]
		public string store;
		public Space space;
		public Vector3Parameter position;
		
		private UnityEngine.Rigidbody rigidbody;

		public override void OnEnter ()
		{
			rigidbody = ownerDefault.GetComponent<UnityEngine.Rigidbody> ();
			Vector3 velocity = Vector3.zero;
			switch (space) {
			case Space.Self:
				velocity=rigidbody.GetRelativePointVelocity (owner.GetValue (position));
				break;
			case Space.World:
				velocity=rigidbody.GetPointVelocity (owner.GetValue (position));
				break;
			}
			owner.SetVector3 (store,velocity);
			Finish ();
		}
		
	}
}
