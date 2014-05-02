using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Math{
	[HideOwnerDefault]
	[Category("Math")]
	[System.Serializable]
	public class Lerp : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		public FloatParameter to;
		public FloatParameter from;

		public override void OnUpdate ()
		{
			float k = Mathf.Lerp (owner.GetValue (from), owner.GetValue (to),Time.deltaTime);
			owner.SetFloat (store, k);
			Finish ();
		}
	}
}