using UnityEngine;
using System.Collections;

namespace AISystem.Actions.NavMeshAgent{
	[Category("NavMeshAgent")]
	[System.Serializable]
	public class ApplyDeltaPosition : BaseAction {
		protected UnityEngine.Animator animator;
		protected UnityEngine.NavMeshAgent agent;
		
		public override void OnEnter ()
		{
			agent = ownerDefault.GetComponent<UnityEngine.NavMeshAgent> ();
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
		}
		
		public override void OnAnimatorMove ()
		{
			agent.velocity = animator.deltaPosition / Time.deltaTime;
			Finish ();
		}
	}
}