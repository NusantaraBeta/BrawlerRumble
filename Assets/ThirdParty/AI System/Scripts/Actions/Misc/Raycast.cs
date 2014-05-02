using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[Category("Misc")]
	[System.Serializable]
	public class Raycast : BaseAction {

		[StoreParameterAttribute(false,typeof(GameObjectParameter))]
		public string storeHitGameObject;
		[StoreParameterAttribute(false,typeof(Vector3Parameter))]
		public string storeHitPoint;
		[StoreParameterAttribute(false,typeof(BoolParameter))]
		public string storeDidHit;

		public LayerMask mask;
		public FloatParameter distance;
		public Space space;
		public Vector3 offset=Vector3.up;
		public Vector3Parameter direction;


		public override void OnUpdate ()
		{
			RaycastHit hit;
			Vector3 dir = space == Space.Self ? ownerDefault.transform.TransformDirection (owner.GetValue (direction)) : owner.GetValue (direction);
			if (Physics.Raycast (ownerDefault.transform.position+offset, dir, out hit, owner.GetValue (distance), mask)) {
				if(storeDidHit != "None"){
					owner.SetBool(storeDidHit,true);
				}
				if(storeHitGameObject!= "None"){
					owner.SetGameObject(storeHitGameObject,hit.transform.gameObject);
				}
				if(storeHitPoint != "None"){
					owner.SetVector3(storeHitPoint,hit.point);
				}
				Debug.Log(hit.transform.name);
			}
			Debug.DrawRay (ownerDefault.transform.position+offset, dir);
			Finish ();
		}
	}
}