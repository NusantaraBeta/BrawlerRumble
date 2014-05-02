using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class AddTorque : BaseAction {
		public ForceMode forceMode;
		public Space space;
		public Vector3Parameter torque;
		
		private UnityEngine.Rigidbody rigidbody;
		
		public override void OnEnter ()
		{
			rigidbody = ownerDefault.GetComponent<UnityEngine.Rigidbody> ();
		}
		
		public override void OnFixedUpdate()
		{
			if (rigidbody != null) {
				switch (space) {
				case Space.World:
					rigidbody.AddTorque (owner.GetValue(torque), forceMode);
					break;
				case Space.Self:
					rigidbody.AddRelativeTorque (owner.GetValue(torque), forceMode);
					break;
				}
			}
			Finish ();
		}
	}
}