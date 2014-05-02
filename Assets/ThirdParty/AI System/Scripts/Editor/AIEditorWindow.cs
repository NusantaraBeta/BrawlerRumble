using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using AISystem;
using AISystem.States;
using AISystem.Actions;

public class AIEditorWindow : GraphEditorWindow {
	public static AIEditorWindow editor;
	public AIController controller;
	private State selectedState;
	private bool isDraggingState;
	private int connectionIndex=-1;
	private int transitionIndex=0;

	public static void Init (AIController controller)
	{
		editor = (AIEditorWindow)EditorWindow.GetWindow (typeof(AIEditorWindow));
		editor.controller = controller;
		editor.title = controller.name;
		State state = editor.controller.states.Find (x => x.GetType() == typeof(AnyState));
		if (state == null) {
			editor.OnCreateStateCallback(new object[]{typeof(AnyState),new Vector2(editor.position.center.x-Node.kNodeWidth*0.5f,editor.position.center.y)});
		}
		editor.selectedState = editor.controller.states.Find (x => x.GetType() == typeof(AnyState));
	}

	protected override void OnGraphGUI ()
	{
		if (controller == null) {
			return;
		}

		if (selectedState == null) {
			selectedState = controller.states.Find (x => x.GetType() == typeof(AnyState));
		}

		if (controller.states.Count > 0) {
			DrawConnections ();
			DrawNodes ();
		}
		HandleCreateEvents ();
		DrawBaseInformation ();
		DrawCustomParameters ();
	}
	
	private void DrawConnections ()
	{
		if (connectionIndex != -1) {
			DrawConnection (controller.states [connectionIndex].position.center, Event.current.mousePosition,position, Color.green);
		}
		if (Event.current.type == EventType.Repaint) {
			foreach (State node in controller.states) {
				if (node.transitions != null) {
					State to=null;
					foreach (BaseTransition target in node.transitions) {
						if(target.toState != to && transitionIndex >=0){
							to=target.toState;
							DrawConnection (target.fromState.position.center,target.toState.position.center,new Rect(position.x+scroll.x,scroll.y+7f,position.width,position.height), controller.states.Find (x => x == node) == selectedState && node.transitions[transitionIndex].toState == target.toState ? Color.cyan : Color.white);
						}
					}
				}
			}
		}
	}

	private void DrawBaseInformation(){
		GUILayout.BeginArea(new Rect (scroll.x, scroll.y, 200, 500));
		bool state = EditorPrefs.GetBool ("BaseInfoFoldout", false);
		if (state) {
			EditorGUIUtility.labelWidth=80;
			GUILayout.BeginVertical ((GUIStyle)"PopupCurveSwatchBackground",GUILayout.Width(199));
			controller.runtimeAnimatorController=(RuntimeAnimatorController)EditorGUILayout.ObjectField("Animator",controller.runtimeAnimatorController,typeof(RuntimeAnimatorController),false);
			GUILayout.EndVertical ();
		}
		if (GUILayout.Button ("Base Information", "flow overlay header upper left", GUILayout.ExpandWidth (true))) {
			EditorPrefs.SetBool("BaseInfoFoldout",!state);	
		}
		GUILayout.EndArea();
	}

	private void DrawCustomParameters(){
		GUILayout.BeginArea(new Rect (scroll.x, scroll.y+position.height-500, 250, 500));
		GUILayout.FlexibleSpace ();
		bool state = EditorPrefs.GetBool ("Parameters", false);

		Rect rect = GUILayoutUtility.GetRect (new GUIContent("Parameters"), "flow overlay header lower left", GUILayout.ExpandWidth (true));
		Rect rect2 = new Rect (rect.x+225,rect.y+2,25,25);

		if (GUI.Button (rect2,"","label")) {
			GenericMenu genericMenu = new GenericMenu ();
			IEnumerable<Type> types= AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()) .Where(type => type.IsSubclassOf(typeof(NamedParameter)));
			foreach(Type type in types){
				genericMenu.AddItem (new GUIContent (type.ToString().Split('.').Last().Replace("Parameter","")), false, new GenericMenu.MenuFunction2 (this.OnCreateParameter),type);
				
			}
			genericMenu.ShowAsContext ();
		}

		if (GUI.Button (rect,"Parameters", "flow overlay header lower left")) {
			EditorPrefs.SetBool("Parameters",!state);	
		}

		GUI.Label (rect2, iconToolbarPlus);

		if (state) {
			//EditorGUIUtility.labelWidth=80;
			GUILayout.BeginVertical ((GUIStyle)"PopupCurveSwatchBackground",GUILayout.Width(249));
			int deleteIndex=-1;
			if(controller.parameters.Count>0){
				for(int i=0;i< controller.parameters.Count;i++){
					NamedParameter parameter= controller.parameters[i];
					GUILayout.BeginHorizontal();
					parameter.Name=EditorGUILayout.TextField(parameter.Name,GUILayout.Width(60));
					if(parameter.GetType() == typeof(BoolParameter)){
						GUILayout.FlexibleSpace();
						BoolParameter boolParam= parameter as BoolParameter;
						boolParam.Value=EditorGUILayout.Toggle(boolParam.Value,GUILayout.Width(14));
					}else if(parameter.GetType() == typeof(TagParameter)){
						TagParameter tagParam= parameter as TagParameter;
						tagParam.Value=EditorGUILayout.TagField(tagParam.Value);
					}else{
						SerializedObject paramObject= new SerializedObject(parameter);
						paramObject.Update();
						EditorGUILayout.PropertyField(paramObject.FindProperty("value"),GUIContent.none);
						paramObject.ApplyModifiedProperties();
					}
					GUILayout.FlexibleSpace();
					if(GUILayout.Button(iconToolbarMinus,"label")){
						deleteIndex=i;
					}
					GUILayout.EndHorizontal();
					
				}
			}else{
				GUILayout.Label("List is Empty");
			}
			if(deleteIndex != -1){
				DestroyImmediate(controller.parameters[deleteIndex],true);
				controller.parameters.RemoveAt(deleteIndex);
				AssetDatabase.SaveAssets();
			}
			GUILayout.EndVertical ();
		}
		GUILayout.EndArea();
	}
	
	private void OnCreateParameter(object data){
		NamedParameter parameter = (NamedParameter)ScriptableObject.CreateInstance ((Type)data);
		AssetDatabase.AddObjectToAsset (parameter, controller);
		AssetDatabase.SaveAssets();
		controller.parameters.Add (parameter);
		if (parameter is ColorParameter) {
			(parameter as ColorParameter).Value=Color.white;
		}else if (parameter is TagParameter) {
			(parameter as TagParameter).Value="Untagged";
		}
		EditorPrefs.SetBool("Parameters",true);	
	}


	private void DrawNodes ()
	{
		foreach (State node in controller.states) {
			if (!node.Equals (selectedState)) {
				DrawNode (node);
			}
		}
		
		DrawNode (selectedState);
		if (isDraggingState) {
			AutoPan (1.0f);
		}
	}

	private void DrawNode (State node)
	{
		bool isTrigger=false;
		object[] attributes=node.GetType().GetCustomAttributes(true);
		foreach(object attribute in attributes){
			if(attribute is TriggerAttribute){
				isTrigger=true;
			}
		}
		UnityEditor.Graphs.Styles.Color color = node.isDefaultState ? UnityEditor.Graphs.Styles.Color.Orange : node.GetType () == typeof(AnyState) ? UnityEditor.Graphs.Styles.Color.Aqua : UnityEditor.Graphs.Styles.Color.Gray;
		if (isTrigger) {
			color=UnityEditor.Graphs.Styles.Color.Green;
		}

		GUI.Box (node.position, node.name, UnityEditor.Graphs.Styles.GetNodeStyle ("node", color, node == selectedState));
		DebugState (node);
		HandleNodeEvents (node);
	}

	private AIRuntimeController lastDebugController;
	private State lastDebugState;
	private void DebugState(State node){
		lastDebugController = (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<AIRuntimeController>() != null) ? Selection.activeGameObject.GetComponent<AIRuntimeController> () : lastDebugController;
		if (EditorApplication.isPlaying && lastDebugController != null && node.id==lastDebugController.CurrentState.id) {
			if(lastDebugState== null || lastDebugState.id != node.id){
				debugProgress=0;
				lastDebugState=node;
			}
			GUI.Box(new Rect(node.position.x+5,node.position.y+20,debugProgress,5),"", "MeLivePlayBar");
		}
	}

	private GameObject lastSelection;
	private float debugProgress;
	private void Update(){
		if (EditorApplication.isPlaying) {
			debugProgress += Time.deltaTime * 30;
			if (debugProgress > 142) {
				debugProgress = 0;
			}
			Repaint ();
		}

		if (Selection.activeGameObject != null &&Selection.activeGameObject != lastSelection && Selection.activeGameObject.GetComponent<AIRuntimeController> () != null) {
			lastSelection=Selection.activeGameObject;
			AIRuntimeController runtime=Selection.activeGameObject.GetComponent<AIRuntimeController>();
			if(runtime.originalController != null){
				Init(runtime.originalController);
			}
		} 
	}


	private void HandleCreateEvents ()
	{
		Event e = Event.current;
		switch (e.type) {
		case EventType.mouseDown:
			if (e.button == 1) {
				GenericMenu genericMenu = new GenericMenu ();
				IEnumerable<Type> types= AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()) .Where(type => type.IsSubclassOf(typeof(State)) && type != typeof(AnyState));
				foreach(Type type in types){
					object[] attributes=type.GetCustomAttributes(true);
					string category=string.Empty;
					bool flag=true;
					bool isTrigger=false;
					foreach(object attribute in attributes){
						if(attribute is CategoryAttribute){
							category=(attribute as CategoryAttribute).Category+"/";
						}
						if(attribute is CanCreateAttribute){
							flag=(attribute as CanCreateAttribute).CanCreate;
						}

						if(attribute is TriggerAttribute){
							isTrigger=true;
						}
					}

					if(flag){
						genericMenu.AddItem (new GUIContent ( (isTrigger?"Create Trigger/":"Create State/")+category+type.ToString().Split('.').Last()), false, new GenericMenu.MenuFunction2 (this.OnCreateStateCallback), new object[] {
							type,
							Event.current.mousePosition
						});
					}
				}
				genericMenu.ShowAsContext ();
				e.Use ();
			}
			break;
		case EventType.mouseUp:
			if (connectionIndex != -1) {
				foreach (State node in controller.states) {
					if (node.position.Contains (e.mousePosition) && node.GetType() != typeof(AnyState)) {

						BaseTransition transition = ScriptableObject.CreateInstance<BaseTransition>();// new BaseTransition (controller.states [connectionIndex].id, node.id, controller);
						transition.fromState=controller.states [connectionIndex];
						transition.toState=node;
						AssetDatabase.AddObjectToAsset (transition, transition.fromState);
						AssetDatabase.SaveAssets();
						transition.fromState.transitions.Add(transition);
					}
				}
				connectionIndex = -1;
			}
			break;
		}
	}
	
	private void HandleNodeEvents (State node)
	{
		Event ev = Event.current;
		switch (ev.type) {
		case EventType.mouseDown:
			if (node.position.Contains (ev.mousePosition) && Event.current.button == 0) {
				isDraggingState = true;
			}

			if (node.position.Contains (ev.mousePosition) && Event.current.button == 1) {
				GenericMenu genericMenu = new GenericMenu ();
				genericMenu.AddItem (new GUIContent ("Make Transition"), false, new GenericMenu.MenuFunction2 (this.MakeTransitionCallback), node);
				if (!((State)node).isDefaultState && node.GetType() != typeof(AnyState) && !(node is BaseTrigger)) {
					genericMenu.AddItem (new GUIContent ("Set As Default"), false, new GenericMenu.MenuFunction2 (this.SetDefaultCallback), node);
				} else {
					genericMenu.AddDisabledItem (new GUIContent ("Set As Default"));
				}
				
				if(node.GetType() == typeof(AnyState)){
					genericMenu.AddDisabledItem (new GUIContent ("Delete"));
				}else{
					genericMenu.AddItem (new GUIContent ("Delete"), false, new GenericMenu.MenuFunction2 (this.DeleteStateCallback), node);
				}

				if(node.GetType() == typeof(AnyState)){
					genericMenu.AddDisabledItem (new GUIContent ("Copy"));
				}else{
					genericMenu.AddItem (new GUIContent ("Copy"), false, new GenericMenu.MenuFunction2 (this.CopyState), node);
				}
				if(copyOfState!= null && node.GetType() != typeof(AnyState) && copyOfState.id != node.id){
					genericMenu.AddItem (new GUIContent ("Paste"), false, new GenericMenu.MenuFunction2 (this.PasteState), node);
				}else{
					genericMenu.AddDisabledItem (new GUIContent ("Paste"));
				}
				genericMenu.ShowAsContext ();
				ev.Use ();
			}
			break;
		case EventType.mouseUp:
			isDraggingState = false;
			Selection.activeObject=node;
			break;
		case EventType.mouseDrag:
			if (isDraggingState) {
				selectedState.position.x += Event.current.delta.x;
				selectedState.position.y += Event.current.delta.y;
				
				if (selectedState.position.y < 10) {
					selectedState.position.y = 10;
				}
				if (selectedState.position.x <  10) {
					selectedState.position.x = 10;
				}
				ev.Use ();
			}
			break;
		}
		
		if (node.position.Contains (ev.mousePosition) && (ev.type != EventType.MouseDown || ev.button != 0 ? false : ev.clickCount == 1)) {
			if (selectedState != node) {
				OnStateSelectionChanged (node);
			}
		}
	}

	public void AutoPan (float speed)
	{
		if (Event.current.type != EventType.repaint) {
			return;
		}
		if (Event.current.mousePosition.x > position.width + scroll.x - 50) {
			//scrollView.x += (speed + 1);
			scroll.x += speed;
			selectedState.position.x += speed;
		}
		
		if ((Event.current.mousePosition.x < scroll.x + 50) && scroll.x > 0) {
			scroll.x -= speed;
			selectedState.position.x -= speed;
		}
		
		if (Event.current.mousePosition.y > position.height + scroll.y - 50) {
		//	scrollView.y += (speed + 1);
			scroll.y += speed;
			selectedState.position.y += speed;
		}
		
		if ((Event.current.mousePosition.y < scroll.y + 50) && scroll.y > 0) {
			scroll.y -= speed;
			selectedState.position.y -= speed;
		}
		Repaint ();
	}

	private State copyOfState;
	private void CopyState(object data){
		copyOfState =(State) ScriptableObject.Instantiate((State)data);

	}

	private void PasteState(object data){
		State dest = (State)data;

		copyOfState.transitions=dest.transitions;
		State state = (State)ScriptableObject.Instantiate ((State)copyOfState);
		state.position = dest.position;
		state.isDefaultState = dest.isDefaultState;
		AssetDatabase.AddObjectToAsset (state, controller);
		AssetDatabase.SaveAssets();
		state.name = state.name.Replace ("(Clone)", "");

		foreach (State node in controller.states) {
			if (node.transitions != null) {
				foreach (BaseTransition transition in node.transitions) {
					if (transition.toState.id == dest.id) {
						transition.toState=state;
					}
				}
			}
		}

		state.id = Guid.NewGuid ().ToString();

		for (int i=0; i< state.transitions.Count; i++) {
			state.transitions[i].fromState=state;
		}

		controller.states.Add (state);

		for (int i=0; i< state.actions.Count; i++) {
			state.actions[i]=(BaseAction)ScriptableObject.Instantiate(state.actions[i]);
			AssetDatabase.AddObjectToAsset (state.actions[i], state);
			AssetDatabase.SaveAssets();
		}

		for (int i=0; i< dest.actions.Count; i++) {
			DestroyImmediate(dest.actions[i],true);
		}
		controller.states.Remove ((State)data);
		DestroyImmediate (dest, true);
		DestroyImmediate (copyOfState);
		AssetDatabase.SaveAssets ();

	}

	private void OnCreateStateCallback (object data)
	{
		object[] mData = (object[])data;
		Type type = (Type)mData [0];
		State state = (State)ScriptableObject.CreateInstance(type);
		state.id = Guid.NewGuid ().ToString();
		state.name = type.ToString().Split('.').Last();
		AssetDatabase.AddObjectToAsset (state, controller);
		AssetDatabase.SaveAssets();

		Vector2 position = (Vector2)mData [1];
		state.position = new Rect (position.x, position.y, Node.kNodeWidth, Node.kNodeHeight);
		controller.states.Add (state);
		selectedState=state;
		Selection.activeObject=selectedState;
		if (controller.states.Find(x=>x.isDefaultState == true) == null && !(state is AnyState) && !(state is BaseTrigger)) {
			state.isDefaultState = true;
		}
	}


	private void MakeTransitionCallback (object userData)
	{
		State state = (State)userData;
		connectionIndex = controller.states.FindIndex (x => x == state);
		
	}
	
	private void SetDefaultCallback (object userData)
	{
		State state = (State)userData;
		State before = controller.states.Find (x => x.isDefaultState == true);
		if (before != null) {
			before.isDefaultState = false;
		}
		state.isDefaultState = true;
	}
	
	private void DeleteStateCallback (object userData)
	{

		State state = (State)userData;
		foreach (State node in controller.states) {
			if (node.transitions != null) {
				List<BaseTransition> removeTransitions = new List<BaseTransition> ();
				foreach (BaseTransition transition in node.transitions) {
					if (transition.toState == state) {
						removeTransitions.Add (transition);
					
					}
				}

				foreach (BaseTransition transition in removeTransitions) {
					DestroyImmediate(transition,true);
					AssetDatabase.SaveAssets();
					node.transitions.Remove(transition);
				}
			}
		}

		if (state == selectedState) {
			selectedState = controller.states.Find (x => x.GetType() == typeof(AnyState));
		}

		for (int i=0; i< state.actions.Count; i++) {
			DestroyImmediate(state.actions[i],true);
		}
		DestroyImmediate(state,true);
		AssetDatabase.SaveAssets();
		controller.states.Remove (state);


	}
	
	private void OnStateSelectionChanged (State state)
	{
		selectedState = controller.states.Find (x => x == state);
		Selection.activeObject = selectedState;
		transitionIndex = 0;
		Event.current.Use ();
	}

	public void TransitionIndexChanged(int index){
		transitionIndex = index;
	
		Repaint ();
	}

	public GUIContent iconToolbarMinus = IconContent("Toolbar Minus", "Remove parameter");
	public GUIContent iconToolbarPlus = IconContent("Toolbar Plus", "Add new parameter");

	public static GUIContent IconContent(string name, string tooltip)
	{
		var t = typeof (EditorGUIUtility);
		var m = t.GetMethod("IconContent", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
		                    Type.DefaultBinder, new[] {typeof (string)}, null);
		var content = (GUIContent) m.Invoke(t, new[] {name});
		content.tooltip = tooltip;
		return content;
	}
}
