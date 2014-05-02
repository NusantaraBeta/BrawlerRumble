using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class SetLookAtPosition : BaseAction {
		public FloatParameter clampWeight;
		public FloatParameter eyesWeight;	
		public FloatParameter headWeight;
		public FloatParameter bodyWeight;
		public FloatParameter weight;

	//	[StoreParameter(true,typeof(Vector3Parameter),typeof(GameObjectParameter))]
		public Vector3Parameter position;

		private UnityEngine.Animator animator;
		
		public override void OnEnter (){
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
		}
		
		public override void OnAnimatorIK (int layerIndex)
		{
			animator.SetLookAtWeight (owner.GetValue(weight), owner.GetValue(bodyWeight), owner.GetValue(headWeight),owner.GetValue( eyesWeight),owner.GetValue( clampWeight));
			animator.SetLookAtPosition (owner.GetValue(position));
			Finish ();
		}
	}
}