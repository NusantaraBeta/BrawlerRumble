using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Debug")]
	[System.Serializable]
	public class DrawLine : BaseAction {
		public Vector3Parameter start;
		public Vector3Parameter end;
		public Color color=Color.red;

		public override void OnUpdate ()
		{
			Debug.DrawLine (owner.GetValue(start),owner.GetValue(end),color);
		
		}
	}
}