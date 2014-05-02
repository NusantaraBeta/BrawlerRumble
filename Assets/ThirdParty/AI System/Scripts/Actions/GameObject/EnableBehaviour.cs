using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("GameObject")]
	[System.Serializable]
	public class EnableBehaviour : BaseAction {
		public bool state;
		public string script;
		
		public override void OnEnter()
		{
			
			MonoBehaviour mComponent = ownerDefault.GetComponent (script) as MonoBehaviour;
			if (mComponent != null) {
				mComponent.enabled=state;
			}
			Finish ();
		}
	}
}