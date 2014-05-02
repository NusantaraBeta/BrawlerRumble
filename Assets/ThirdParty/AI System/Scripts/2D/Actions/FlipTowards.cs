using UnityEngine;
using System.Collections;


namespace AISystem.Actions{
	[Category("2D")]
	[System.Serializable]
	public class FlipTowards : BaseAction {
		public GameObjectParameter target;
		
		private GameObject mTarget;
		private SpriteAgent agent;
		private bool facingRight;
		public override void OnEnter ()
		{
			if (mTarget == null) {
				mTarget = owner.GetValue(target);
			}
			agent = ownerDefault.GetComponent<SpriteAgent> ();
			facingRight = ownerDefault.transform.localScale.x > 0;
		}
		
		
		public override void OnUpdate ()
		{
			if (mTarget != null) {
				Vector2 dir = new Vector2(mTarget.transform.position.x,mTarget.transform.position.y) - agent.position;
				if (dir.x > 0.1f) { // face right
					Flip (true);
				}else if(dir.x<-0.1f){
					Flip (false);
				}
				
			}
			Finish ();
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