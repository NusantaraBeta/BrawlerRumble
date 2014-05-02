using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("Transform")]
	[System.Serializable]
	public class LookAt : BaseAction {
		public Vector3 ignore=Vector3.up;
		public Vector3 offset;
		[StoreParameter(true,typeof(GameObjectParameter))]
		public string target;

		private GameObject mTarget;

		public override void OnEnter ()
		{
			mTarget = owner.GetGameObject(target);
			if (mTarget != null && ownerDefault != null) {
				Vector3 position = mTarget.transform.position;
				Vector3 ownerPosition = ownerDefault.transform.position;
				
				position.x = (ignore.x > 0 ? ownerPosition.x : position.x);
				position.y = (ignore.y > 0 ? ownerPosition.y : position.y);
				position.z = (ignore.z > 0 ? ownerPosition.z : position.z);
				ownerDefault.transform.LookAt ((position + offset));
			} 
			Finish ();
		}
	}
}