using UnityEngine;
using System.Collections;

namespace AISystem.Actions.NavMeshAgent{
	[Category("NavMeshAgent")]
	[System.Serializable]
	public class Stop : BaseAction {
		private UnityEngine.NavMeshAgent agent;
		
		public override void OnEnter ()
		{
			agent = ownerDefault.GetComponent<UnityEngine.NavMeshAgent> ();
			agent.Stop ();
			Finish ();
		}
	}
}