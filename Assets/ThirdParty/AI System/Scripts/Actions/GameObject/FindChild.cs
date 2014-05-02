using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("GameObject")]
	[System.Serializable]
	public class FindChild : BaseAction {
		[StoreParameter(true,typeof(GameObjectParameter))]
		public string store;
		public string childName;

		public override void OnEnter ()
		{
			Transform child = Find (ownerDefault.transform);
			if (child != null) {
				owner.SetGameObject(store,child.gameObject);
			}
			Finish ();
		}

		private Transform Find(Transform target)
		{
			if (target.name == childName) 
				return target;
			
			for (int i = 0; i < target.transform.childCount; ++i)
			{
				Transform result = Find(target.GetChild(i));
				
				if (result != null) 
					return result;
			}
			return null;
		}
	}
}