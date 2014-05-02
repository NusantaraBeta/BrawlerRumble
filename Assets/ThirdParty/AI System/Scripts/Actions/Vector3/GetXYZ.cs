using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Vector3")]
	[System.Serializable]
	public class GetXYZ : BaseAction {
		[StoreParameter(false,typeof(FloatParameter))]
		public string z;
		[StoreParameter(false,typeof(FloatParameter))]
		public string y;
		[StoreParameter(false,typeof(FloatParameter))]
		public string x;
		public Vector3Parameter vector;

		public override void OnEnter ()
		{
			Vector3 v = owner.GetValue (vector);
			if (x != "None") {
				owner.SetFloat(x,v.x);
			}
			if (y != "None") {
				owner.SetFloat(y,v.y);
			}
			if (z != "None") {
				owner.SetFloat(z,v.z);
			}
			Finish ();
		}
	}
}