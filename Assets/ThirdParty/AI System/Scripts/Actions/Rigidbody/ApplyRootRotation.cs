using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class ApplyRootRotation : BaseAction {
		protected UnityEngine.Animator animator;
		protected UnityEngine.Rigidbody rigidbody;
		public override void OnAwake ()
		{
			rigidbody = ownerDefault.GetComponent<UnityEngine.Rigidbody> ();
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
		}
		
		public override void OnAnimatorMove ()
		{
			rigidbody.rotation = animator.rootRotation;
			Finish ();
		}
	}
}