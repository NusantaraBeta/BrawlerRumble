using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[HideOwnerDefault]
	[Category("Time")]
	[System.Serializable]
	public class WaitForSeconds : BaseAction {
		[MinMax(0,20)]
		public MinMax seconds;

		private float time;
		public override void OnEnter ()
		{
			time = UnityEngine.Time.time + seconds.GetRandom ();
		}

		public override void OnUpdate ()
		{
			if (UnityEngine.Time.time > time) {
				Finish();
			}
		}
	}
}