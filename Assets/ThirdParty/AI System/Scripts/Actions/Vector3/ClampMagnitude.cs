using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Vector3")]
	[System.Serializable]
	public class ClampMagnitude : BaseAction {
		[StoreParameter(true,typeof(Vector3Parameter))]
		public string store;
		public FloatParameter maxLength;
		public Vector3Parameter vector;
	

		public override void OnEnter ()
		{
			owner.SetVector3 (store, Vector3.ClampMagnitude( owner.GetValue(vector),owner.GetValue(maxLength)));
			Finish ();
		}
	}
}