using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Time")]
	[System.Serializable]
	public class SmoothDeltaTime : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		public override void OnEnter ()
		{
			owner.SetFloat (store,UnityEngine.Time.smoothDeltaTime);
			Finish ();
		}
	}
}