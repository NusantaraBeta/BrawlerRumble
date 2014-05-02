using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class AddForce : BaseAction {
		public ForceMode forceMode;
		public Space space;
		public Vector3Parameter force;

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
					rigidbody.AddForce (owner.GetValue(force), forceMode);
					break;
				case Space.Self:
					rigidbody.AddRelativeForce (owner.GetValue(force), forceMode);
					break;
				}
			}
			Finish ();
		}
	}
}