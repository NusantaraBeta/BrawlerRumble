using UnityEngine;
using System.Collections;

namespace AISystem.States.Sprites{
	[Category("2D")]
	[CanCreate(false)]
	[System.Serializable]
	public class Movement : Simple {
		public float speed;
		public float threshold=0.5f;
		[AnimatorParameter(AnimatorParameter.State)]
		public string jumpState;

		private bool facingRight = true;
		protected Rigidbody2D rigidbody2D;
		protected SpriteAgent agent;
		protected Animator animator;

		public override void OnAwake ()
		{
			agent = owner.GetComponent<SpriteAgent> ();
			rigidbody2D = owner.GetComponent<Rigidbody2D> ();
			animator = owner.GetComponent<Animator> ();
		}

		private bool traversingLink;
		protected virtual void DoMovement(Vector2 position){
			if (agent.isOnJumpLink) {
				if(!traversingLink){
					animator.CrossFade(jumpState,0);
					agent.position=agent.currentJumpLink.start.position;
					rigidbody2D.velocity=TrajectoryVelocity(agent.currentJumpLink.start.position,agent.currentJumpLink.end.position,agent.currentJumpLink.traverseTime);
					traversingLink=true;
				}

				if(Vector2.Distance(agent.position,agent.currentJumpLink.end.position)<0.1f){
					agent.isOnJumpLink=false;
					traversingLink=false;
				}
			} else {
				Vector2 dir = position - agent.position;
				facingRight = owner.transform.localScale.x > 0;
				if (dir.x > 0.1f) {
					Flip (true);
				} else if (dir.x < -0.1f) {
					Flip (false);
				}
				rigidbody2D.velocity = new Vector2 ((facingRight ? 1 : -1) * speed, rigidbody2D.velocity.y);
			}
		}

		public override void OnExit ()
		{
			agent.isOnJumpLink=false;
			traversingLink=false;
		}

		
		private Vector3 TrajectoryVelocity(Vector3 origin, Vector3 target, float timeToTarget) {
			Vector3 toTarget = target - origin;
			Vector3 toTargetXZ = toTarget;
			toTargetXZ.y = 0;

			float y = toTarget.y;
			float xz = toTargetXZ.magnitude;

			float t = timeToTarget;
			float v0y = y / t +  0.5f*Physics2D.gravity.magnitude*rigidbody2D.gravityScale *t;
			float v0xz = xz / t;

			Vector3 result = toTargetXZ.normalized;
			result *= v0xz;
			result.y = v0y;
			return result;
		}


		void Flip (bool facingRight)
		{
			if (this.facingRight != facingRight) {
				// Switch the way the player is labelled as facing.
				this.facingRight = facingRight;
				
				// Multiply the player's x local scale by -1.
				Vector3 theScale = owner.transform.localScale;
				theScale.x *= -1;
				owner.transform.localScale = theScale;
			}
		}
	}
}