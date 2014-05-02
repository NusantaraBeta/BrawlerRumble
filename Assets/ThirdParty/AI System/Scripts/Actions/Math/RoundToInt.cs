using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Math{
	[HideOwnerDefault]
	[Category("Math")]
	[System.Serializable]
	public class RoundToInt : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		public FloatParameter value;
		
		
		
		public override void OnEnter ()
		{
			owner.SetFloat (store,Mathf.RoundToInt(owner.GetValue (value)));
			Finish ();
		}
	}
}