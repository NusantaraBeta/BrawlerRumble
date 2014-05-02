using UnityEngine;
using System.Collections;

namespace AISystem{
	[System.Serializable]
	public class GameObjectParameter : NamedParameter {
		[SerializeField]
		private GameObject value;
		
		public GameObject Value
		{
			get{
				return this.value;
			}
			set{
				this.value = value;
			}
		}

		public Vector3 Position
		{
			get{
				return this.Value.transform.position;
			}

		}
	}
}