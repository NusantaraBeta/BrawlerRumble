using UnityEngine;
using System.Collections;

namespace AISystem.Actions.NavMeshAgent{
	[Category("NavMeshAgent")]
	[System.Serializable]
	public class ApplyRootRotation : BaseAction {
		protected UnityEngine.Animator animator;
		protected UnityEngine.NavMeshAgent agent;

		public override void OnEnter ()
		{
			agent = ownerDefault.GetComponent<UnityEngine.NavMeshAgent> ();
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
			agent.updateRotation = false;
		}
		
		public override void OnAnimatorMove ()
		{
			ownerDefault.transform.rotation=animator.rootRotation;
			Finish ();
		}
	}
}