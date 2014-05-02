using UnityEngine;
using System.Collections;


namespace AISystem.Actions{
	[Category("Transform")]
	[System.Serializable]
	public class Rotate : BaseAction {
		public Space space;
		public Vector3Parameter eulerAngles;
		private Transform mTransform;

		public override void OnEnter ()
		{
			mTransform = ownerDefault.transform;
		}

		public override void OnUpdate ()
		{
			if (mTransform != null) {
				mTransform.Rotate (owner.GetValue(eulerAngles) * Time.deltaTime,space);		
			}
			Finish ();
		}
	}
}