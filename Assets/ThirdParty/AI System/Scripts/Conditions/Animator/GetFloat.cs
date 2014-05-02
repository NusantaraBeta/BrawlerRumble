using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("Animator")]
	[System.Serializable]
	public class GetFloat : BaseCondition {
		[AnimatorParameter(AnimatorParameter.Float)]
		public string parameterName;
		public FloatComparer comparer;
		public float value;

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
				float floatValue=animator.GetFloat(hash);
				switch (comparer) {
				case FloatComparer.Greater:
					return floatValue > value;
				case FloatComparer.Less:
					return floatValue < value;
				}
			}
			return false;
		}
	}
}