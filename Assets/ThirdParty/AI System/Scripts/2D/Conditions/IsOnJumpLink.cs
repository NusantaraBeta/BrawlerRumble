using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("2D")]
	[System.Serializable]
	public class IsOnJumpLink : BaseCondition {
		private SpriteAgent agent;
		public bool equals;

		public override void OnAwake ()
		{
			agent = owner.GetComponent<SpriteAgent> ();
		}
		
		public override bool Validate ()
		{
			return agent.isOnJumpLink == equals;
		}
	}
}