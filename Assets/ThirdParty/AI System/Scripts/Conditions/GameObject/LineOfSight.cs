using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("GameObject")]
	[System.Serializable]
	public class LineOfSight : BaseCondition {
		[StoreParameter(true,typeof(GameObjectParameter))]
		public string target;
		public float angle=160.0f;
		public LayerMask mask;
		public Vector3 offset;
		public bool equals;

		private GameObject mTarget;
		
		public override void OnEnter ()
		{
			mTarget = ((GameObjectParameter)owner.GetParameter (target)).Value;
		}

		public override bool Validate ()
		{
			if(mTarget == null){
				mTarget = ((GameObjectParameter)owner.GetParameter (target)).Value;
				return equals == false;
			}
			float targetAngle = Vector3.Angle (mTarget.transform.position - owner.transform.position,owner.transform.forward);
			if (Mathf.Abs (targetAngle) < angle*0.5f) {
				RaycastHit hit;
				if (Physics.Linecast (owner.transform.position + offset, mTarget.transform.position + offset, out hit, mask)) {
					if (hit.transform == mTarget.transform) {  
						return equals==true;
					}
				}
			}
			return equals == false;
		}
	}
}