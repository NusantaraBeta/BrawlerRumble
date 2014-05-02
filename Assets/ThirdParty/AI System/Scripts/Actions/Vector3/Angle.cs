using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Vector3")]
	[System.Serializable]
	public class Angle : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		public Vector3Parameter to;
		public Vector3Parameter from;

		public override void OnEnter ()
		{
			owner.SetFloat (store, Vector3.Angle(owner.GetValue(from),owner.GetValue(to)));
			Finish ();
		}
	}
}