using UnityEngine;
using System.Collections;

namespace AISystem.States{

	[CanCreate(true)]
	[System.Serializable]
	public class OnTriggerEnter : BaseTrigger {
		[Tag]
		public string triggerTag="Untagged";
	}
}