using UnityEngine;
using System.Collections;

namespace AISystem.Actions.NavMeshAgent{
	[Category("NavMeshAgent")]
	[System.Serializable]
	public class GetVelocity : BaseAction {
		
		[StoreParameter(true,typeof(Vector3Parameter))]
		public string store;
		private UnityEngine.NavMeshAgent agent;

		public override void OnEnter ()
		{
			agent = ownerDefault.GetComponent<UnityEngine.NavMeshAgent> ();
			owner.SetVector3 (store, agent.velocity);
			Finish ();
		}
	}
}