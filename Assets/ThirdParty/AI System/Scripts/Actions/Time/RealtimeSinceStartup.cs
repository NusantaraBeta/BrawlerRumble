using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Time")]
	[System.Serializable]
	public class RealtimeSinceStartup : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		public override void OnEnter ()
		{
			owner.SetFloat (store,UnityEngine.Time.realtimeSinceStartup);
			Finish ();
		}
	}
}