using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("GameObject")]
	[System.Serializable]
	public class Distance : BaseCondition {
		[StoreParameter(true,typeof(GameObjectParameter))]
		public string target;
		public FloatComparer comparer;
		public float value;
		public Vector3 scale=Vector3.one;
		private GameObject mTarget;

		public override bool Validate ()
		{
			mTarget =owner.GetGameObject(target);
			if (mTarget != null) {
				Vector3 ownerPos=owner.transform.position;
				ownerPos.Scale(scale);
				Vector3 targetPos=mTarget.transform.position;
				targetPos.Scale(scale);
				float distance = Vector3.Distance (ownerPos, targetPos);
				switch (comparer) {
				case FloatComparer.Greater:
					return distance > value;
				case FloatComparer.Less:
					return distance < value;
				}
			}
			return false;
		}
	}
}