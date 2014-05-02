using UnityEngine;
using System.Collections;

namespace AISystem.States.Sprites{
	[CanCreate(true)]
	[System.Serializable]
	public class Walk : Movement {
		public float range=10.0f;

		private Vector2 initialPosition;
		private Vector2 walkPosition;
		public override void OnAwake ()
		{
			base.OnAwake ();
			initialPosition = agent.position;
			walkPosition = agent.position;
		}

		public override void OnFixedUpdate ()
		{
			if (Vector2.Distance(agent.position,walkPosition)<threshold) {
				walkPosition = new Vector2 (initialPosition.x + Random.Range (-range, range), agent.position.y); 
			}
			Debug.DrawLine (agent.position, walkPosition);
			DoMovement (walkPosition);
		}

		public override void OnExit ()
		{
			base.OnExit ();
			walkPosition = agent.position;
		}
	}
}
