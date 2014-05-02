using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class ApplyDeltaPosition: BaseAction {
		protected UnityEngine.Animator animator;
		protected UnityEngine.Rigidbody rigidbody;
		public float speedMultiplier=1;

		public override void OnAwake ()
		{
			rigidbody = ownerDefault.GetComponent<UnityEngine.Rigidbody> ();
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
		}
		
		public override void OnAnimatorMove ()
		{
			rigidbody.velocity = (animator.deltaPosition*speedMultiplier) / Time.deltaTime;
			Finish ();
		}
	}
}