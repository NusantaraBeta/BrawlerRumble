using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("Misc")]
	[System.Serializable]
	public class CompareFloat : BaseCondition {
		public FloatParameter first;
		public FloatComparer comparer;
		public FloatParameter second;
		
		public override bool Validate ()
		{


			switch(comparer){
			case FloatComparer.Greater:
				return owner.GetValue(first) > owner.GetValue(second);
			case FloatComparer.Less:
				return owner.GetValue(first) < owner.GetValue(second);
			}
			
			return false;
		}
	}
}