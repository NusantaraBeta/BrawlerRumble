using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Vector3")]
	[System.Serializable]
	public class Subtract : BaseAction {
		[StoreParameter(true,typeof(Vector3Parameter))]
		public string store;
		public Vector3Parameter second;
		public Vector3Parameter first;
		
		
		public override void OnEnter ()
		{
			owner.SetVector3 (store, owner.GetValue(first)-owner.GetValue(second));
			Finish ();
		}
	}
}