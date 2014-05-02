using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Math{
	[HideOwnerDefault]
	[Category("Math")]
	[System.Serializable]
	public class Divide : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		public FloatParameter second;
		public FloatParameter first;
		
		
		
		public override void OnEnter ()
		{
			float s = owner.GetValue (second);
			if (s < 0.01f) {
				s=0.01f;
			}
			owner.SetFloat (store, owner.GetValue (first) / s);
			Finish ();
		}
	}
}