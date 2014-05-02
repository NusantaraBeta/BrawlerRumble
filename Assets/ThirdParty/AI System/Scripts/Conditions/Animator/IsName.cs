using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("Animator")]
	[System.Serializable]
	public class IsName : BaseCondition {
		public int layer;
		[AnimatorParameter(AnimatorParameter.State)]
		public string stateName;
		public bool equals;

		private Animator animator;

		public override void OnAwake ()
		{
			animator = owner.GetComponent<Animator> ();
		}

		public override bool Validate ()
		{
			if(animator != null){
				AnimatorStateInfo stateInfo=animator.GetCurrentAnimatorStateInfo(layer);
				return (stateInfo.IsName(stateName) ==equals);
			}
			return false;
		}
	}
}