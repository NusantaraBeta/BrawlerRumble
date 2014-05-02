using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class GetBool : BaseAction {
		[StoreParameter(true,typeof(BoolParameter))]
		public string store;
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
			owner.SetBool (store, animator.GetBool (hash));
			Finish ();
		}
	}
}