using UnityEngine;
using System.Collections;

namespace AISystem{
	[System.Serializable]
	public class NamedParameter:ScriptableObject {
		public bool userVariable;
		[SerializeField]
		private string parameterName;
		public string Name{
			get{
				return parameterName;
			}
			set{
				parameterName=value;
			}
		}
		
		private void OnEnable(){
			hideFlags = HideFlags.HideInHierarchy;
		}
	}
}