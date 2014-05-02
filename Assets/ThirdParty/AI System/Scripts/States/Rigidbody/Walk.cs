using UnityEngine;
using System.Collections;

namespace AISystem.States.Rigidbody{
	[CanCreate(true)]
	[System.Serializable]
	public class Walk : Movement {
		public float range=10.0f;
		public float threshold=0.1f;		
		
		private Vector3 initialPosition;
		private Vector3 randomPosition;

		public override void OnAwake ()
		{
			base.OnAwake ();
			initialPosition = owner.transform.position;
			randomPosition = initialPosition;
		}

		public override void OnFixedUpdate ()
		{
			Debug.Log (Vector3.Distance (owner.transform.position, randomPosition));
			Debug.DrawLine (owner.transform.position, randomPosition);
			if (Vector3.Distance (owner.transform.position, randomPosition) < threshold) {
				randomPosition=GetRandomDestination(true);
			}
			DoMovement (randomPosition);
		}

		public override void OnExit ()
		{
			base.OnExit ();
			randomPosition = owner.transform.position;
		}
		
		private Vector3 GetRandomDestination(bool raycast){
			Vector3 random = new Vector3 (initialPosition.x + Random.Range (-range, range), initialPosition.y, initialPosition.z + Random.Range (-range, range)); 
			if (raycast) {
				RaycastHit hit;
				if (Physics.Raycast (random + Vector3.up * 500, Vector3.down, out hit)) {
					random.y = hit.point.y;
				}
			}
			return random;
		}
	}
}