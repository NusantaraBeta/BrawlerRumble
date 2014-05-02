using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("GameObject")]
	[System.Serializable]
	public class IsNull : BaseCondition {
		[StoreParameter(true,typeof(GameObjectParameter))]
		public string target;
		public bool equals;

		public override bool Validate ()
		{
			return (((GameObjectParameter)owner.GetParameter (target)).Value == null) == equals;
		}
	}
}
