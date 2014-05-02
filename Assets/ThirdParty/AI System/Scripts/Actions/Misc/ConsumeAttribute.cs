using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Misc")]
	[System.Serializable]
	public class ConsumeAttribute : BaseAction {
		public int amount;
		[AttributePopup]
		public string attribute;
		
		public override void OnEnter ()
		{
			BaseAttribute mAttribute= owner.GetAttribute (attribute);
			if (mAttribute != null) {
				mAttribute.Consume(amount);
			}
			Finish ();
		}
	}
}