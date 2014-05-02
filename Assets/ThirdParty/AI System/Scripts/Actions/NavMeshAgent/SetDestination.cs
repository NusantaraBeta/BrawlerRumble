using UnityEngine;
using System.Collections;

namespace AISystem.Actions.NavMeshAgent{
	[Category("NavMeshAgent")]
	[System.Serializable]
	public class SetDestination : BaseAction {
		
		[StoreParameter(true,typeof(Vector3Parameter),typeof(GameObjectParameter))]
		public string destination;
		private UnityEngine.NavMeshAgent agent;
		
		public override void OnEnter ()
		{
			agent = ownerDefault.GetComponent<UnityEngine.NavMeshAgent> ();
			agent.SetDestination (owner.GetVector3 (destination));
			Finish ();
		}
	}
}