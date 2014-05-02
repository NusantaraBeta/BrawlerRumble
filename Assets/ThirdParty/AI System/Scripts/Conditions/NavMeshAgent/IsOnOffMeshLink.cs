using UnityEngine;
using System.Collections;

namespace AISystem.Conditions.NavMeshAgent{
	[Category("NavMeshAgent")]
	[System.Serializable]
	public class IsOnOffMeshLink : BaseCondition {
		public bool equals;
		private UnityEngine.NavMeshAgent agent;
		public override void OnAwake ()
		{
			agent = owner.GetComponent<UnityEngine.NavMeshAgent> ();
		}

		public override bool Validate ()
		{
			return agent.isOnOffMeshLink == equals;
		}
	}
}