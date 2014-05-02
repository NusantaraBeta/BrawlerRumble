using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Math{
	[HideOwnerDefault]
	[Category("Math")]
	[System.Serializable]
	public class Subtract : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		public FloatParameter second;
		public FloatParameter first;
		
		
		
		public override void OnEnter ()
		{
			owner.SetFloat (store, owner.GetValue (first) - owner.GetValue (second));
			Finish ();
		}
	}
}