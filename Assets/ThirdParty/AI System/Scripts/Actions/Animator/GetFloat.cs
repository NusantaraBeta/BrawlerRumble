using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class GetFloat : BaseAction {
		[StoreParameter(true,typeof(FloatParameter))]
		public string store;
		[AnimatorParameterAttribute(AnimatorParameter.Float)]
		public string parameterName;
		
		private UnityEngine.Animator animator;
		private int hash;
		
		public override void OnAwake (){
			hash = UnityEngine.Animator.StringToHash (parameterName);
		}
		
		public override void OnEnter ()
		{
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
			owner.SetFloat (store, animator.GetFloat (hash));
			Finish ();
		}
	}
}