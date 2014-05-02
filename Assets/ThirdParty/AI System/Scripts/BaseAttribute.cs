using UnityEngine;
using System.Collections;

namespace AISystem{
	[System.Serializable]
	public class BaseAttribute {
		public string name;
		public AnimationCurve maxValue;
		public float multiplier=100.0f;
		private int level;
		private int curValue;
		public delegate void AttributeChangedEvent(int value);
		private event AttributeChangedEvent onAttributeChanged;

		public void Initialize(int level){
			this.level = level;
			this.curValue = MaxValue;
		}

		public int CurValue{
			get{
				return curValue;
			}
		}

		public int MaxValue{
			get{
				return (int)(maxValue.Evaluate(level*0.01f)*multiplier);
			}
		}

		public AttributeChangedEvent OnAttributeChanged{
			get{
				return onAttributeChanged;
			}
			set{
				onAttributeChanged+=value;
			}
		}

		
		/// <summary>
		/// Substract a value from CurValue and retruns true if less or equal zero
		/// </summary>
		public bool Consume(int val){
			curValue -= val;
			curValue = Mathf.Clamp (curValue,0, MaxValue);
			if (onAttributeChanged != null) {
				onAttributeChanged (curValue);
			}
			return (curValue < 1);
		}
		
		/// <summary>
		/// Add a value and retruns true if CurValue is MaxValue
		/// </summary>
		public bool Add(int val){
			curValue += val;
			curValue = Mathf.Clamp (curValue, 0, MaxValue);
			if (onAttributeChanged != null) {
			//	onAttributeChanged (curValue);
			}
			return (curValue == MaxValue);
		}
		
		public BaseAttribute(int level){
			this.level = level;
		}
		
		public BaseAttribute(BaseAttribute other, int level){
			this.name = other.name;
			this.maxValue = other.maxValue;
			this.multiplier = other.multiplier;
			this.level = level;
			this.curValue = MaxValue;
		}
	}
}