using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("Transform")]
	[System.Serializable]
	public class GetProjectedVelocity : BaseAction {
		[StoreParameter(false,typeof(FloatParameter))]
		public string magnitude;
		[StoreParameter(false,typeof(Vector3Parameter))]
		public string velocity;
		[StoreParameter(true,typeof(Vector3Parameter),typeof(GameObjectParameter))]
		public string target;
		
		public override void OnEnter ()
		{
			Vector3 dir = owner.GetVector3 (target) - ownerDefault.transform.position;
			Vector3 vel = Vector3.Project (dir, ownerDefault.transform.forward);
			if (magnitude != "None") {
				owner.SetFloat(magnitude,vel.magnitude);
			}
			if (velocity != "None") {
				owner.SetVector3(velocity,vel);
			}
			Finish ();
		}
		
	
	}
}