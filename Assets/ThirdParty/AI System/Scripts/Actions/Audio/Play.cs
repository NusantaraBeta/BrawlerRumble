using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Audio{
	[Category("Audio")]
	[System.Serializable]
	public class Play : BaseAction {
		public float maxDistance=500.0f;
		public float minDistance=3.0f;
		public float volume=1;
		public AudioClip audioClip;

		public override void OnEnter ()
		{
			AudioSource audio=ownerDefault.GetComponent<AudioSource>();
			if(audio == null){
				audio=ownerDefault.AddComponent<AudioSource>();
			}
			audio.volume = volume;
			audio.minDistance = minDistance;
			audio.maxDistance = maxDistance;
			audio.clip=audioClip;
			audio.Play();
			Finish ();
		}
	}
}