using UnityEngine;
using System.Collections;


namespace AISystem.Actions{
	[Category("Transform")]
	[System.Serializable]
	public class Translate : BaseAction {
		public Space space;
		public Vector3 direction=Vector3.forward;

		public override void OnUpdate ()
		{
			if (ownerDefault != null) {
				ownerDefault.transform.Translate (direction * Time.deltaTime,space);		
			}
			Finish ();
		}
	}
}