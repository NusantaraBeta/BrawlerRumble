using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("Transform")]
	[System.Serializable]
	public class GetDirection : BaseAction {
		[StoreParameter(false,typeof(Vector3Parameter))]
		public string normalized;
		[StoreParameter(false,typeof(FloatParameter))]
		public string magnitude;
		[StoreParameter(false,typeof(Vector3Parameter))]
		public string direction;
		[StoreParameter(true,typeof(Vector3Parameter),typeof(GameObjectParameter))]
		public string target;
		
		public override void OnEnter ()
		{
			Vector3 dir = owner.GetVector3 (target) - ownerDefault.transform.position;
			if (magnitude != "None") {
				owner.SetFloat(magnitude,dir.magnitude);
			}

			if (normalized != "None") {
				owner.SetVector3(normalized,dir.normalized);
			}

			if (direction != "None") {
				owner.SetVector3(direction,dir);
			}
			Finish ();
		}
		
		
	}
}