using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Vector3")]
	[System.Serializable]
	public class SqrMagnitude : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		public Vector3Parameter vector;
		
		public override void OnEnter ()
		{
			owner.SetFloat (store, owner.GetValue(vector).sqrMagnitude);
			Finish ();
		}
	}
}