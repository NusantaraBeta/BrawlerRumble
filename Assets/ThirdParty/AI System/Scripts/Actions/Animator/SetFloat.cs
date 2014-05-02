using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class SetFloat : BaseAction {
		public float dampTime=0.15f;
		[AnimatorParameter(AnimatorParameter.Float)]
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

			if (dampTime > 0) {
				animator.SetFloat (hash, owner.GetValue(value), dampTime, Time.deltaTime);
			} else {
				animator.SetFloat (hash,  owner.GetValue(value));
			}
			Finish ();
		}
		
	}
}