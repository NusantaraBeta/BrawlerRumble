using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Parameter{
	[HideOwnerDefault]
	[Category("Parameter")]
	[System.Serializable]
	public class SetFloat : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		[MinMax(-20.0f,20.0f)]
		public MinMax value;

		public override void OnEnter ()
		{
			owner.SetFloat(store,value.GetRandom());
			Finish ();
		}
	}
}