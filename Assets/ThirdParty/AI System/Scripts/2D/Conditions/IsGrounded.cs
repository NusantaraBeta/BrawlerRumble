using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("2D")]
	[System.Serializable]
	public class IsGrounded : BaseCondition {
		public LayerMask mask;
		public float groundedRadius=0.2f;
		private SpriteAgent agent;
		public bool equals;

		public override void OnAwake ()
		{
			agent = owner.GetComponent<SpriteAgent> ();
		}

		public override bool Validate ()
		{
			return Physics2D.OverlapCircle(agent.position, groundedRadius, mask) == equals;
		}
	}
}