using UnityEngine;
using System.Collections;

namespace AISystem.States.Sprites{
	[CanCreate(true)]
	[System.Serializable]
	public class Follow : Movement {
		[StoreParameter(true,typeof(GameObjectParameter))]
		public string target;
		
		protected GameObject mTarget;
		public override void OnEnter ()
		{
			mTarget = owner.GetGameObject (target);
		}
		
		public override void OnFixedUpdate()
		{
			if (mTarget != null) {
				DoMovement (mTarget.transform.position);
			}
		}
	}
}