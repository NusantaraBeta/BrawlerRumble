using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class SetBool : BaseAction {
		public BoolParameter value;
		[AnimatorParameterAttribute(AnimatorParameter.Bool)]
		public string parameterName;


		private UnityEngine.Animator animator;
		private int hash;
		
		public override void OnAwake (){
			hash = UnityEngine.Animator.StringToHash (parameterName);
		}
		
		public override void OnEnter ()
		{
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
			animator.SetBool (hash, owner.GetValue(value));
			Finish ();
		}
	}
}