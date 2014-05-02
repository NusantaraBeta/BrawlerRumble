using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("GameObject")]
	[System.Serializable]
	public class Instantiate : BaseAction {
		[StoreParameter(false,typeof(GameObjectParameter))]
		public string store;
		public Vector3Parameter angle;
		public Vector3Parameter position;
		public GameObjectParameter prefab;

		public override void OnEnter ()
		{
			GameObject go=(GameObject)GameObject.Instantiate(owner.GetValue(prefab),owner.GetValue(position),Quaternion.Euler(owner.GetValue(angle)));
			if (store != "None") {
				owner.SetGameObject (store, go);
			} 
			Finish ();
		}
	}
}