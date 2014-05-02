using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class UpdateManager : EditorWindow {

	[MenuItem ("Window/Update Manager")]
	public static void Init () {
		UpdateManager manager=(UpdateManager)EditorWindow.GetWindow (typeof (UpdateManager));
		manager.invoice = PlayerPrefs.GetString ("AIInvoice", string.Empty);
		manager.checkForUpdates = PlayerPrefs.GetInt ("checkForUpdates", 1) > 0 ? true : false;
		if (!string.IsNullOrEmpty (manager.invoice)) {
			manager.loading=true;
			WWWForm form= new WWWForm();
			form.AddField("invoice",manager.invoice);
			var www = new WWW ("http://zerano-unity3d.com/checkUpdates.php",form);
			
			ContinuationManager.Add (() => www.isDone, () =>
			                         {
				
				if (!string.IsNullOrEmpty (www.error)) {
					Debug.Log ("WWW failed: " + www.error);
				}
				
				if (!www.text.Trim().Equals("false")) {
					string[] all = www.text.Split (',');
					manager.examples = new List<string> (all);
					PlayerPrefs.SetString("AIInvoice",manager.invoice);
					manager.hasIncoice = true;
					PlayerPrefs.SetString("AIUpdate",System.DateTime.Today.ToString());
				}
				manager.loading=false;
			});
		}
	}

	private List<string> examples;
	private string invoice;
	private bool hasIncoice;
	private int cnt;
	private bool loading;
	private bool checkForUpdates;
	private void OnGUI(){
		if (!hasIncoice && !loading) {
			invoice = EditorGUILayout.TextField ("Invoice number:", invoice);
			if (GUILayout.Button ("Continue")) {
				loading=true;
				WWWForm form= new WWWForm();
				form.AddField("invoice",invoice.Trim());
				var www = new WWW ("http://zerano-unity3d.com/checkUpdates.php",form);

				ContinuationManager.Add (() => www.isDone, () =>
				                         {

					if (!string.IsNullOrEmpty (www.error)) {
						Debug.Log ("WWW failed: " + www.error);
					}

					if (!www.text.Trim().Equals("false")) {
						string[] all = www.text.Split (',');
						examples = new List<string> (all);
						PlayerPrefs.SetString("AIInvoice",invoice.Trim());
						hasIncoice = true;
					}
					loading=false;
					cnt++;
				});
			}

			if(cnt>0 && !loading && !hasIncoice){
				GUILayout.Label("Incorrect invoice number please try again or contact the publisher.");
			}
		} else {
			if (examples != null) {
				foreach (string s in examples) {
					GUILayout.BeginHorizontal ("box");
					string[] v=s.Split ('/').Last ().Split(';');
					GUILayout.BeginHorizontal();

					GUI.color=PlayerPrefs.GetString(v[0]) != v[1]?Color.red:Color.white;
					GUILayout.Label ((PlayerPrefs.GetString(v[0]) != v[1]?"New ":"")+ v[0].Replace(".unitypackage",""),GUILayout.Width(180));
					GUI.color=Color.white;

					GUILayout.Label(v[1]);
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace ();
					if (examples.Count > 1) {
						if (GUILayout.Button ("Download")) {
							PlayerPrefs.SetString(v[0],v[1]);
							Application.OpenURL (s.Replace(";"+v[1],""));
						}
					}
					GUILayout.EndHorizontal ();
				}
				checkForUpdates=EditorGUILayout.Toggle("Check For Updates",checkForUpdates);
				PlayerPrefs.SetInt("checkForUpdates",checkForUpdates==true?1:0);
			} else{
				GUILayout.Label("Loading...");
			}
		}
	}
}
