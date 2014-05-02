using UnityEngine;
using System.Collections;

namespace AISystem.States{
	[CanCreate(true)]
	[System.Serializable]
	public class OnAttributeChanged : BaseTrigger {
	
		public BaseAttribute attribute;

		public override void OnAwake ()
		{
			attribute.Initialize (owner.level);
			attribute.OnAttributeChanged = AttributeChangedCallback;
		}

		public void AttributeChangedCallback(int value){
			//Debug.Log ("Attribute " + attribute.name + " changed " + value);
			owner.TryEnterState (this);
		}
	}
}