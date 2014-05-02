using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class SetInteger : BaseAction {
		[AnimatorParameter(AnimatorParameter.Int)]
		public string parameterName;
		public FloatParameter value;
		
		private UnityEngine.Animator animator;
		private int hash;
		
		public override void OnAwake (){
			hash = UnityEngine.Animator.StringToHash (parameterName);
		}
		
		public override void OnEnter ()
		{
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
			animator.SetInteger (hash,  Mathf.RoundToInt(owner.GetValue(value)));

			Finish ();
		}
		
	}
}