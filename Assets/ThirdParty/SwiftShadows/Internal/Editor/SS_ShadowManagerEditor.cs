using UnityEditor;
using UnityEngine;
using LostPolygon.Internal.SwiftShadows;

#if !UNITY_3_5
using LostPolygon.SwiftShadows;
namespace LostPolygon.Internal.SwiftShadows {
#endif
/// <summary>
/// Displays the shadow manager statistics.
/// </summary>
[CustomEditor(typeof (SS_ShadowManager))]
public class SS_ShadowManagerEditor : Editor {
    private SS_ShadowManager _object;

    private void OnEnable() {
        _object = target as SS_ShadowManager;
    }

    public override void OnInspectorGUI() {
        if (_object == null) {
            return;
        }

        Repaint();

        GUILayout.Label("Statistics:", EditorStyles.boldLabel);

        if (_object.ShadowManagers.Count == 0) {
            GUILayout.Label("No shadows are registered");
        }

        foreach (SS_ShadowMeshManager meshManager in _object.ShadowManagers) {
            EditorGUILayout.BeginVertical();
            GUILayout.Label(string.Format("Material: {0} (id: {1})", meshManager.Material.name, meshManager.Material.GetInstanceID()));
            GUILayout.Label(string.Format("Static: {0}", meshManager.IsStatic));
            GUILayout.Label(string.Format("Layer: {0}", LayerMask.LayerToName(meshManager.Layer)));
            GUILayout.Label(string.Format("Shadows total: {0}", meshManager.ShadowsCount));
            GUILayout.Label(string.Format("Shadows visible: {0}", meshManager.VisibleShadowsCount));
            EditorGUILayout.ObjectField("Mesh preview: ", meshManager.Mesh, typeof(Mesh), false);
            GUILayout.Space(20);
            EditorGUILayout.EndVertical();
        }
    }
}
#if !UNITY_3_5
}
#endif