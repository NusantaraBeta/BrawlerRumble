using UnityEngine;
using System.Collections;


namespace AISystem.Actions.Parameter{
	[HideOwnerDefault]
	[Category("Parameter")]
	[System.Serializable]
	public class SetGameObject : BaseAction {
		[StoreParameter(true,typeof(GameObjectParameter))]
		public string store;
		public GameObjectParameter setGameObject;
		
		public override void OnEnter ()
		{
			owner.SetGameObject (store, owner.GetValue (setGameObject));
			Finish ();
		}
	}
}