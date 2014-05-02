using UnityEngine;
using System.Collections;


namespace AISystem.States.Sprites{
	[CanCreate(true)]
	[System.Serializable]
	public class Flee : Movement {
		[StoreParameter(true,typeof(GameObjectParameter))]
		public string target;
		
		protected GameObject mTarget;
		public override void OnEnter ()
		{
			mTarget = owner.GetGameObject (target);
			Debug.Log ("Enter");
		}
		
		public override void OnFixedUpdate()
		{
			if (mTarget != null) {
				float dir = mTarget.transform.position.x - agent.position.x;
				DoMovement (new Vector2 (agent.position.x + 5 * (dir > 0 ? -1 : 1), agent.position.y));
				
			} else {
				Debug.Log("target null");			
			}
		}
	
	}
}