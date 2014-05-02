using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("Animator")]
	[System.Serializable]
	public class IsInTransition : BaseCondition {
		public int layer;
		public bool equals;

		private Animator animator;
		public override void OnAwake ()
		{
			animator = owner.GetComponent<Animator> ();
		}
		
		public override bool Validate ()
		{
			if(animator != null){
				return animator.IsInTransition(layer) == equals;
			}
			return false;
		}
	}
}
