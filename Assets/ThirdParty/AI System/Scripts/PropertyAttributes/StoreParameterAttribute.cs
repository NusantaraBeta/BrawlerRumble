using UnityEngine;
using System.Collections;
using System;

public class StoreParameterAttribute:PropertyAttribute {
	public string _default="None";
	public bool required;
	public bool constant;
	public Type[] types;

	public StoreParameterAttribute(bool required,string _default,params Type[] types){
		this.required = required;
		this.types = types;
		this._default = _default;
	}

	public StoreParameterAttribute(bool required,params Type[] types){
		this.required = required;
		this.types = types;
	}

	public StoreParameterAttribute(bool required,bool constant,string _default,params Type[] types){
		this.required = required;
		this.types = types;
		this._default = _default;
		this.constant = constant;
	}
	
	public StoreParameterAttribute(bool required,bool constant,params Type[] types){
		this.required = required;
		this.types = types;
		this.constant=constant;
	}
}
