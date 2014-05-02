using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("Transform")]
	[System.Serializable]
	public class SmoothLookAt : BaseAction {
		public Vector3 ignore=Vector3.up;
		public float speed;
		public Vector3Parameter target;

		private Quaternion lastRotation;
		private Quaternion desiredRotation;
		public override void OnUpdate ()
		{
			if (ownerDefault != null) {
				Vector3 position = owner.GetValue(target);
				Vector3 ownerPosition = ownerDefault.transform.position;
				
				position.x = (ignore.x > 0 ? ownerPosition.x : position.x);
				position.y = (ignore.y > 0 ? ownerPosition.y : position.y);
				position.z = (ignore.z > 0 ? ownerPosition.z : position.z);

				Vector3 diff = position - ownerDefault.transform.position;
				if (diff != Vector3.zero && diff.sqrMagnitude > 0)
				{
					desiredRotation = Quaternion.LookRotation(diff);
				}
				
				lastRotation = Quaternion.Slerp(lastRotation, desiredRotation, speed * Time.deltaTime);
				ownerDefault.transform.rotation = lastRotation;
			} 
			Finish ();
		}
	}
}