using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("Misc")]
	[System.Serializable]
	public class AttributeValue : BaseCondition {
		[AttributePopup]
		public string attribute;
		public FloatComparer comparer;
		public float value;

		public override bool Validate ()
		{
			BaseAttribute mAttribute = owner.GetAttribute (attribute);
			if (mAttribute != null) {
				switch(comparer){
				case FloatComparer.Greater:
					return mAttribute.CurValue > value;
				case FloatComparer.Less:
					return mAttribute.CurValue < value;
				}
			}
			return false;
		}
	}
}