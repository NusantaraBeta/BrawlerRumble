using UnityEngine;
using System.Collections;

namespace AISystem{
	[System.Serializable]
	public class AudioClipParameter : NamedParameter {
		[SerializeField]
		private AudioClip value;
		
		public AudioClip Value
		{
			get{
				return this.value;
			}
			set{
				this.value = value;
			}
		}
	}
}