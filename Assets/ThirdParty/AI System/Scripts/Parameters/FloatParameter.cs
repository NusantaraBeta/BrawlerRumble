using UnityEngine;
using System.Collections;

namespace AISystem{
	[System.Serializable]
	public class FloatParameter : NamedParameter {
		[SerializeField]
		private float value;
		
		public float Value
		{
			get{
				return this.value;
			}
			set{
				this.value = value;
			}
		}
	}
}