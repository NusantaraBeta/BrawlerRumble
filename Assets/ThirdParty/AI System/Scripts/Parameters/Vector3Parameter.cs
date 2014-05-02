using UnityEngine;
using System.Collections;

namespace AISystem{
	[System.Serializable]
	public class Vector3Parameter : NamedParameter {
		[SerializeField]
		private Vector3 value;
		
		public Vector3 Value
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