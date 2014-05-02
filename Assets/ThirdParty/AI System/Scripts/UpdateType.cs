using UnityEngine;
using System.Collections;

namespace AISystem{
	public enum UpdateType {
		OnUpdate,
		OnFixedUpdate,
		OnLateUpdate,
		OnAnimatorIK,
		OnAnimatorMove
	}
}