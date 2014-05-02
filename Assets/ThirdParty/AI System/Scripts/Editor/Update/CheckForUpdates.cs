using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
///
[InitializeOnLoad]
public class CheckForUpdates
{
	static CheckForUpdates()
	{
		EditorApplication.update += Run;
	}
	
	static void Run()
	{
		EditorApplication.update -= Run;
		if (!EditorApplication.isPlaying && PlayerPrefs.GetString("AIUpdate",string.Empty) != System.DateTime.Today.ToString()) {
			string invoice = PlayerPrefs.GetString ("AIInvoice", string.Empty);
			if (!string.IsNullOrEmpty (invoice) && PlayerPrefs.GetInt ("checkForUpdates") > 0) {
				WWWForm form = new WWWForm ();
				form.AddField ("invoice", invoice);
				var www = new WWW ("http://zerano-unity3d.com/checkUpdates.php", form);


				ContinuationManager.Add (() => www.isDone, () =>
				                         {
					if (!string.IsNullOrEmpty (www.error)) {
						Debug.Log ("WWW failed: " + www.error);
					}
					
					if (!www.text.Trim ().Equals ("false")) {
						string[] all = www.text.Split (',');
						List<string> examples = new List<string> (all);
						foreach (string s in examples) {
							string[] v = s.Split ('/').Last ().Split (';');
							if (PlayerPrefs.GetString (v [0]) != v [1]) {
								UpdateManager.Init ();
								break;
							}
							
						}
					}
				});
			}
		}
	}
}
