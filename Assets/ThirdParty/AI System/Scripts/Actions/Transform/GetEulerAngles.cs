using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("Transform")]
	[System.Serializable]
	public class GetEulerAngles : BaseAction {
		[StoreParameter(true,typeof(Vector3Parameter))]
		public string store;
		
		public override void OnEnter ()
		{
			owner.SetVector3 (store, ownerDefault.transform.rotation.eulerAngles);
			Finish ();
		}
	}
}