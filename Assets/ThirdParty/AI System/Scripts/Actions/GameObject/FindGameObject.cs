using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("GameObject")]
	[System.Serializable]
	public class FindGameObject : BaseAction {
		[Tag()]
		public string tag="Untagged";
		[StoreParameter(true,typeof(GameObjectParameter))]
		public string store;

		public override void OnEnter ()
		{
			owner.SetGameObject(store, GameObject.FindGameObjectWithTag (tag));
			Finish ();
		}
	}
}