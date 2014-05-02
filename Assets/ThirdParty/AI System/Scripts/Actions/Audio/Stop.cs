using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Audio{
	[Category("Audio")]
	[System.Serializable]
	public class Stop : BaseAction {
		public override void OnEnter ()
		{
			AudioSource audio=ownerDefault.GetComponent<AudioSource>();
			if(audio != null){
				audio.Stop();
			}
			Finish ();
		}
	}
}