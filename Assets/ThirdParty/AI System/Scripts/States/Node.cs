using UnityEngine;
using System.Collections;

namespace AISystem.States{
	[System.Serializable]
	public class Node : ScriptableObject {
		public const float kNodeWidth = 150f;
		public const float kNodeHeight = 30f;
		public Rect position;
		public string id;

		private void OnEnable(){
			hideFlags = HideFlags.HideInHierarchy;
		}
	}
}