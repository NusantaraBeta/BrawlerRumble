using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("GameObject")]
	[System.Serializable]
	public class CompareGameObject : BaseCondition {
		public GameObjectParameter first;
		public GameObjectParameter second;
		public bool equals;

		public override bool Validate ()
		{
			return (owner.GetValue (first).transform == owner.GetValue (second).transform) == equals;
		}
	}
}