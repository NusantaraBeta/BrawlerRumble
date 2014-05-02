using System;

namespace AISystem{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class CanCreateAttribute : Attribute
	{
		private readonly bool canCreate;
		
		public bool CanCreate
		{
			get
			{
				return this.canCreate;
			}
		}

		public CanCreateAttribute(bool canCreate){
			this.canCreate=canCreate;
		}
	}
}