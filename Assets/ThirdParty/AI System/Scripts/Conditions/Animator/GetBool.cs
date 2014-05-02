using UnityEngine;
using System.Collections;


namespace AISystem{
	[Category("Animator")]
	[System.Serializable]
	public class GetBool : BaseCondition {
		[AnimatorParameter(AnimatorParameter.Bool)]
		public string parameterName;
		public bool equals;
		
		private Animator animator;
		private int hash;
		public override void OnAwake ()
		{
			animator = owner.GetComponent<Animator> ();
			hash = Animator.StringToHash (parameterName);
		}
		
		public override bool Validate ()
		{
			if(animator != null){
				return animator.GetBool(hash) == equals;
			}
			return false;
		}
	}
}