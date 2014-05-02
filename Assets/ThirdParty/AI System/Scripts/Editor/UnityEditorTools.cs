#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class UnityEditorTools
{
	/// <summary>
	/// Gets the names of the scenes, added to the build settings.
	/// </summary>
	public static string[] BuildScenes {
		get {
			List<string> temp = new List<string> ();
			foreach (UnityEditor.EditorBuildSettingsScene s in UnityEditor.EditorBuildSettings.scenes) {
				if (s.enabled) {
					string name = s.path.Substring (s.path.LastIndexOf ('/') + 1);
					name = name.Substring (0, name.Length - 6);
					temp.Add (name);
				}
			}
			return temp.ToArray ();
		}
	}

	/// <summary>
	/// Gets the name of the current loaded scene.
	/// </summary>
	public static string CurrentScene {
		get {
			string currentLoadedScene = string.Empty;
			currentLoadedScene = EditorApplication.currentScene.Substring (EditorApplication.currentScene.LastIndexOf ('/') + 1);
			currentLoadedScene = currentLoadedScene.Substring (0, currentLoadedScene.Length - 6);
			return currentLoadedScene;
		}
	}
	
	/// <summary>
	/// Creates a custom asset
	/// </summary>
	public static T CreateAsset<T> (bool displayFilePanel) where T : ScriptableObject
	{
		T asset = null;
		string mPath = EditorUtility.SaveFilePanelInProject (
         	       "Create Asset of type " + typeof(T).ToString (),
            	   	"New " + typeof(T).ToString () + ".asset",
                	"asset", "");
		asset = CreateAsset<T> (mPath);
		return asset;
	}
	
	/// <summary>
	/// Creates a custom asset
	/// </summary>
	public static T CreateAsset<T> () where T : ScriptableObject
	{
		T asset = null;
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
         
		if (path == "") {
			path = "Assets";
		} else if (System.IO.Path.GetExtension (path) != "") {
			path = path.Replace (System.IO.Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString () + ".asset");
		asset = CreateAsset<T> (assetPathAndName);
		return asset;
	}
	
	/// <summary>
	/// Creates a custom asset
	/// </summary>
	public static T CreateAsset<T> (string path) where T : ScriptableObject
	{
		if (string.IsNullOrEmpty (path)) {
			return null;
		}
		T data = null;
		data = ScriptableObject.CreateInstance<T> ();
		AssetDatabase.CreateAsset (data, path);
		AssetDatabase.SaveAssets ();
		return data;
	}
	
	/// <summary>
	/// Draws all properties
	/// </summary>
	public static void DrawSerializedProperties(SerializedObject obj){
		SerializedProperty prop= obj.GetIterator();

		//Hide the script property
		prop.NextVisible(true);
		
		while (prop.NextVisible(true)){
			EditorGUILayout.PropertyField(prop);
		}
		obj.ApplyModifiedProperties();
	}

	public static void DrawSerializedProperties(SerializedProperty prop){
		//Hide the script property
		//prop.NextVisible(true);
		
		while (prop.NextVisible(true)){
			EditorGUILayout.PropertyField(prop);
		}
		prop.serializedObject.ApplyModifiedProperties();
	}
	
	public static string SearchField(string search, out bool changed, params GUILayoutOption[] options){
		GUILayout.BeginHorizontal (GUILayout.Width (180));
		string before = search;
		string after = EditorGUILayout.TextField ("", before, "SearchTextField",GUILayout.Width (168));
		
		if (GUILayout.Button ("", "SearchCancelButton", GUILayout.Width (18f))) {
			after = string.Empty;
			GUIUtility.keyboardControl = 0;
		}
		GUILayout.EndHorizontal();
		
		changed= before != after;
		
		return after;
	}


	public static string StringPopup(Rect position, string value, string[] list){
		int index=0;
		if(list != null && list.Length>0){
			for(int cnt=0; cnt<list.Length;cnt++){
				if(value == list[cnt]){
					index=cnt;
				}
			}
			index=EditorGUI.Popup(position,index,list);
			return list[index];
		}
		return string.Empty;
	}

	public static string StringPopup(Rect position,string label, string value, string[] list){
		int index=0;
		if(list != null && list.Length>0){
			for(int cnt=0; cnt<list.Length;cnt++){
				if(value == list[cnt]){
					index=cnt;
				}
			}
			index=EditorGUI.Popup(position,label,index,list);
			return list[index];
		}
		return string.Empty;
	}

	public static string StringPopup(string value, string[] list,params GUILayoutOption[] options){
		return StringPopup ("", value, list,options);
	}

	public static string StringPopup(string label,string value, string[] list,params GUILayoutOption[] options){
		int index=0;
		if(list != null && list.Length>0){
			for(int cnt=0; cnt<list.Length;cnt++){
				if(value == list[cnt]){
					index=cnt;
				}
			}
			index=string.IsNullOrEmpty(label)?EditorGUILayout.Popup(index,list,options):EditorGUILayout.Popup(label,index,list,options);
			return list[index];
		}
		return string.Empty;
	}
}
#endif