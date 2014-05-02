using System.Collections;


namespace AISystem.Actions.Rigidbody{
	[Category("Rigidbody")]
	[System.Serializable]
	public class IsSleeping : BaseAction {
		[StoreParameter(true,typeof(BoolParameter))]
		public string store;
		
		private UnityEngine.Rigidbody rigidbody;
		
		public override void OnEnter ()
		{
			rigidbody = ownerDefault.GetComponent<UnityEngine.Rigidbody> ();
			owner.SetBool (store,rigidbody.IsSleeping ());
			Finish ();
		}
		
	}
}
