using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("GameObject")]
	[System.Serializable]
	public class SendMessage : BaseAction {
		public Object objectParameter;
		public int intParameter;
		public float floatParameter;
		public string stringParameter;
		public MessageParameterType parameter;
		public string message;

		public override void OnEnter ()
		{
			switch (parameter) {
			case MessageParameterType.Float:
				ownerDefault.SendMessage(message,floatParameter,SendMessageOptions.DontRequireReceiver);
				break;
			case MessageParameterType.Int:
				ownerDefault.SendMessage(message,intParameter,SendMessageOptions.DontRequireReceiver);
				break;
			case MessageParameterType.String:
				ownerDefault.SendMessage(message,stringParameter,SendMessageOptions.DontRequireReceiver);
				break;
			case MessageParameterType.Object:
				ownerDefault.SendMessage(message,objectParameter,SendMessageOptions.DontRequireReceiver);
				break;
			default:
				ownerDefault.SendMessage(message,SendMessageOptions.DontRequireReceiver);
				break;
			}
			Finish ();
		}

		public enum MessageParameterType{
			None,
			Int,
			Float,
			String,
			Object
		}
	}
}