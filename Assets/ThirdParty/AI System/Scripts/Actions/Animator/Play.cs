using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class Play : BaseAction {
		public int layer;
		[AnimatorParameter(AnimatorParameter.State)]
		public string stateName;


		private UnityEngine.Animator animator;
		private int hash;
		
		public override void OnAwake (){
			hash = UnityEngine.Animator.StringToHash (stateName);
		}

		public override void OnEnter ()
		{
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
			animator.Play (hash, layer);
			Finish ();
		}
	}
}