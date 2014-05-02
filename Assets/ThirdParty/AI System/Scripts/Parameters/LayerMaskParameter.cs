using UnityEngine;
using System.Collections;

namespace AISystem{
	[System.Serializable]
	public class LayerMaskParameter : NamedParameter {
		[SerializeField]
		private LayerMask value;
		
		public LayerMask Value
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