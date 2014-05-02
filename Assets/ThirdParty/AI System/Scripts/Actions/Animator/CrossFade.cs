using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class CrossFade : BaseAction {
		public int layer;
		public float transitionDuration;
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
			animator.CrossFade (hash, transitionDuration, layer);
			Finish ();
		}
	}
}