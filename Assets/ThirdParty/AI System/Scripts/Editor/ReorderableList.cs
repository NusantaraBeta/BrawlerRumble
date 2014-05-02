using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;


public class ReorderableList {
	public delegate void AddCallbackDelegate();
	public ReorderableList.AddCallbackDelegate onAddCallback;
	public delegate void RemoveCallbackDelegate(int index);
	public ReorderableList.RemoveCallbackDelegate onRemoveCallback;
	public delegate void ElementCallbackDelegate(int index);
	public ReorderableList.ElementCallbackDelegate drawElementCallback;
	public delegate void SelectCallbackDelegate(int index);
	public ReorderableList.SelectCallbackDelegate onSelectCallback;
	public delegate void OnHeaderClick();
	public ReorderableList.OnHeaderClick onHeaderClick;

	private string title;
	private IList items;
	private bool draggable;
	private int selectedIndex = -2;
	private bool isDragging;
	private bool displayAdd;
	private bool doFoldOut;

	public ReorderableList(string title):this(null,title,false,false,false){
		
	}

	public ReorderableList(IList items, string title,bool draggable):this(items,title,draggable,true,true){

	}

	public ReorderableList(IList items, string title,bool draggable, bool displayAdd, bool foldOut){
		this.title = title;
		this.items = items;
		this.draggable = draggable;
		this.displayAdd = displayAdd;
		this.doFoldOut = foldOut;
	}

	public void DoList(){
		if (DoListHeader ()) {
			DoListItems();
		}
	}

	public bool DoListHeader(){
		bool foldOut = EditorPrefs.GetBool (title, false);
		Rect rect = GUILayoutUtility.GetRect (new GUIContent (title), "flow overlay header lower left", GUILayout.ExpandWidth (true));
		rect.x -= 1;
		rect.width += 2;
		Rect rect2 = new Rect (rect.width-10,rect.y+2,25,25);

		if (GUI.Button (rect2,"","label") && onAddCallback != null && displayAdd) {
			onAddCallback();
		}

		
		if (GUI.Button (rect, title, "flow overlay header lower left")) {
			if(Event.current.button==0){
				EditorPrefs.SetBool (title, !foldOut);	
			}
			if(Event.current.button == 1 && onHeaderClick != null){
				onHeaderClick();
			}
		}

		if (displayAdd) {
			GUI.Label (rect2, iconToolbarPlus);
		}
		return foldOut;
	}
	
	private void DoListItems(){
		//EditorGUIUtility.labelWidth = 80;
		GUILayout.BeginVertical ((GUIStyle)"PopupCurveSwatchBackground", GUILayout.ExpandWidth (true));
		int swapIndex=-1;
		int removeIndex = -1;
		
		if (items.Count > 0) {
			for (int i=0; i< items.Count; i++) {
				GUI.enabled = !(i == selectedIndex);
				GUILayout.BeginHorizontal ();
				GUILayout.Space (15);
		
				GUILayout.BeginVertical ();
				GUILayout.Box(GUIContent.none,"PopupCurveSwatchBackground",GUILayout.ExpandWidth(true),GUILayout.Height(1));
				bool foldOut = EditorPrefs.GetBool (title + items [i].GetHashCode (), false);
				
				GUILayout.BeginHorizontal ();
				GUI.SetNextControlName (title + i);
				if (!doFoldOut && drawElementCallback != null) {
					drawElementCallback (i);
				}

				object[] attributes=items[i].GetType().GetCustomAttributes(true);
				string category=string.Empty;
				foreach(object attribute in attributes){
					if(attribute is AISystem.CategoryAttribute){
						category=(attribute as AISystem.CategoryAttribute).Category;
						break;
					}
				}

				string foldoutLabel=category+"."+items[i].GetType().ToString().Split('.').Last();

				foldOut = doFoldOut ? EditorGUILayout.Foldout (foldOut, foldoutLabel) : true;
				EditorPrefs.SetBool (title + items [i].GetHashCode (), foldOut);	
				
				GUILayout.FlexibleSpace ();
				
				Rect r = GUILayoutUtility.GetRect (20, 20);
				r.x += 5;
				if (GUI.Button (r, iconToolbarMinus, "label")) {
					removeIndex = i;
				}
				
				GUILayout.EndHorizontal ();
				
				if (doFoldOut && foldOut && drawElementCallback != null) {
					drawElementCallback (i);
				}

				//GUILayout.Box(GUIContent.none,"PopupCurveSwatchBackground",GUILayout.ExpandWidth(true),GUILayout.Height(1));
				GUILayout.EndVertical ();
				GUILayout.EndHorizontal ();
				GUI.enabled = true;
				//if (this.draggable) {
					Rect elementRect = GUILayoutUtility.GetLastRect ();
					switch (Event.current.type) {
					case EventType.MouseDown:
						if (elementRect.Contains (Event.current.mousePosition)) {
							if (onSelectCallback != null) {
								onSelectCallback (i);
							}
							GUI.FocusControl (title + i);
							if(draggable && items.Count>1){
								selectedIndex = i;
								isDragging = true;
							}
						}
						break;
					case EventType.MouseUp:
						if (selectedIndex != i && elementRect.Contains (Event.current.mousePosition) && draggable) {
							swapIndex = i;
						}
						isDragging = false;
						break;
					case EventType.MouseDrag:
						if (elementRect.Contains (Event.current.mousePosition)) {
							GUI.FocusControl (title + i);
						}
						break;
					}
				}
			//}
		} else {
			GUILayout.Label("List is Empty");
		}
		if (swapIndex != -1) {
			items.Swap (selectedIndex, swapIndex);
			selectedIndex = -2;
		}
		
		if (!isDragging) {
			selectedIndex = -2;
		}
		
		if(removeIndex != -1 && onRemoveCallback != null){
			onRemoveCallback(removeIndex);
		}
		GUILayout.EndVertical ();
		GUI.enabled = true;
	}

	public GUIContent iconToolbarMinus = IconContent("Toolbar Minus", "Remove action");
	public GUIContent iconToolbarPlus = IconContent("Toolbar Plus", "Add new action");
	
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
