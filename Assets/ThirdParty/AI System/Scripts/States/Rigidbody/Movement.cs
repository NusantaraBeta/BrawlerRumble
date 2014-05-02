using UnityEngine;
using System.Collections;

namespace AISystem.States.Rigidbody{
	[CanCreate(false)]
	[Category("Rigidbody")]
	[System.Serializable]
	public class Movement : Simple {
		public float speed=2;
		public float rotation=5;
		public float gravity=2;

		protected Animator animator;
		protected UnityEngine.Rigidbody rigidbody;
		private Quaternion lastRotation;
		private Quaternion desiredRotation;

		public override void OnAwake ()
		{
			rigidbody = owner.GetComponent<UnityEngine.Rigidbody> ();
			animator = owner.GetComponent<Animator> ();
		}

		protected virtual void DoMovement(Vector3 position){
			Vector3 dir = position - owner.transform.position;
			dir.y = -gravity;
			rigidbody.AddForce (owner.transform.forward * speed,ForceMode.VelocityChange);
			dir.y = 0;
			if (dir != Vector3.zero && dir.sqrMagnitude > 0)
			{
				desiredRotation = Quaternion.LookRotation(dir,Vector3.up);
			}
			
			lastRotation = Quaternion.Slerp(lastRotation, desiredRotation, rotation * Time.deltaTime);
			owner.transform.rotation = lastRotation;
		}
	}
}