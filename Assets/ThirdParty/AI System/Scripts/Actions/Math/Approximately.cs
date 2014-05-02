using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Math{
	[HideOwnerDefault]
	[Category("Math")]
	[System.Serializable]
	public class Approximately : BaseAction {
		[StoreParameter(true,typeof(BoolParameter))]
		public string store;
		public FloatParameter second;
		public FloatParameter first;
		
		
		
		public override void OnEnter ()
		{
			owner.SetBool (store, Mathf.Approximately(owner.GetValue (first),owner.GetValue (second)));
			Finish ();
		}
	}
}