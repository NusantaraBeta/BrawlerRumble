using UnityEngine;
using System.Collections;

namespace AISystem.States.NavMeshAgent{
	[CanCreate(false)]
	[Category("NavMeshAgent")]
	[System.Serializable]
	public class Movement : Simple {

		public float speed=2.0f;
		public float rotation=150.0f;
		public bool applyRootMotion;

		protected Animator animator;
		protected UnityEngine.NavMeshAgent agent;

		public override void OnAwake ()
		{
			agent = owner.GetComponent<UnityEngine.NavMeshAgent> ();
			animator = owner.GetComponent<Animator> ();
		}

		public override void OnEnter ()
		{
			agent.speed = speed;
			agent.angularSpeed = rotation;
		}

		public override void OnExit ()
		{
			agent.Stop ();
			agent.SetDestination(owner.transform.position);
		}
		
		public override void OnAnimatorMove ()
		{
			agent.updateRotation = !applyRootMotion;
			if (applyRootMotion) {
				owner.transform.rotation=animator.rootRotation;
				if(agent != null){
					agent.velocity = animator.deltaPosition / Time.deltaTime;
				}
			}
		}
	}
}