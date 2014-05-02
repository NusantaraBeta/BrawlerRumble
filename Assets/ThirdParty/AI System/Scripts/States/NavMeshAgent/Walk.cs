using UnityEngine;
using System.Collections;

namespace AISystem.States.NavMeshAgent{
	[CanCreate(true)]
	[System.Serializable]
	public class Walk : Movement {
		public float range=10.0f;
		public float threshold=0.1f;		

		private Vector3 initialPosition;

		public override void OnAwake ()
		{
			base.OnAwake ();
			initialPosition = owner.transform.position;
		}

		public override void OnUpdate ()
		{
			if (agent.remainingDistance < threshold) {
				Vector3 destination=GetRandomDestination(true);
				NavMeshHit hit;
				NavMesh.SamplePosition(destination, out hit, range, 1);
				destination = hit.position;
				agent.SetDestination(destination);
			}
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