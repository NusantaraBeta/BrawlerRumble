using UnityEngine;
using System.Collections;

namespace AISystem{
	[Category("Misc")]
	[System.Serializable]
	public class ExitTime : BaseCondition {
		[MinMax(0,50)]
		public MinMax seconds;
		private float exitTime;

		public override void OnEnter ()
		{

			exitTime = Time.time + seconds.GetRandom ();
		//	Debug.Log ("Exit");
		}

		public override bool Validate (){
			return Time.time > exitTime;
		}
	}
}