using UnityEngine;
using System.Collections;


namespace AISystem.Actions.Parameter{
	[HideOwnerDefault]
	[Category("Parameter")]
	[System.Serializable]
	public class SetBool : BaseAction {
		[StoreParameter(true,typeof(BoolParameter))]
		public string store;
		public BoolParameter state;
		
		public override void OnEnter ()
		{
			owner.SetBool (store, owner.GetValue (state));
			Finish ();
		}
	}
}