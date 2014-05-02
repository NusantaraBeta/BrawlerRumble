using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Time")]
	[System.Serializable]
	public class TimeScale : BaseAction {
		public FloatParameter value;

		public override void OnEnter ()
		{
			UnityEngine.Time.timeScale = owner.GetValue (value);
			Finish ();
		}
	}
}