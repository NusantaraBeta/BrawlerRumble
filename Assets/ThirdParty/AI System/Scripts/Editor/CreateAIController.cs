using UnityEngine;
using UnityEditor;
using System.Collections;
using AISystem;

public static class CreateAIController {
	[MenuItem("Assets/Create/AIController")]
	public static void CreateAIControllerAsset()
	{
		UnityEditorTools.CreateAsset<AIController>();
	}
}
