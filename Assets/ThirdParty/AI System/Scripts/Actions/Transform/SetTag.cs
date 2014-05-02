using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("Transform")]
	[System.Serializable]
	public class SetTag : BaseAction {
		[Tag()]
		public string tag="Untagged";

		public override void OnEnter ()
		{
			ownerDefault.transform.tag = tag;
			Finish ();
		}
	}
}