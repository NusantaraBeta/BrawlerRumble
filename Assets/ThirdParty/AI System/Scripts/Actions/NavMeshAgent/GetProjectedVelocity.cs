using UnityEngine;
using System.Collections;

namespace AISystem.Actions.NavMeshAgent{
	[Category("NavMeshAgent")]
	[System.Serializable]
	public class GetProjectedVelocity : BaseAction {
		[StoreParameter(false,typeof(Vector3Parameter))]
		public string velocity;
		[StoreParameter(false,typeof(FloatParameter))]
		public string magnitude;

		private UnityEngine.NavMeshAgent agent;

		public override void OnEnter ()
		{
			agent = ownerDefault.GetComponent<UnityEngine.NavMeshAgent> ();
			Vector3 vel = Vector3.Project (agent.desiredVelocity, ownerDefault.transform.forward);
			if (velocity != "None") {
				owner.SetVector3 (velocity, vel);
			}
			if (magnitude != "None") {
//				Debug.Log(vel.magnitude);
				owner.SetFloat (magnitude, vel.magnitude);
			}
			Finish ();
		}
	}
}