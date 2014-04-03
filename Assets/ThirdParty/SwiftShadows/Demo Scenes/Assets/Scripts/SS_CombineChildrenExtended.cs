#pragma warning disable 0618

//recreated by Neodrop. 
//mailto : neodrop@unity3d.ru

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
Attach this script as a parent to some game objects. The script will then combine the meshes at startup.
This is useful as a performance optimization since it is faster to render one big mesh than many small meshes. See the docs on graphics performance optimization for more info.

Different materials will cause multiple meshes to be created, thus it is useful to share as many textures/material as you can.
*/
//[ExecuteInEditMode()]
[AddComponentMenu("Mesh/Combine Children")]
public class SS_CombineChildrenExtended : MonoBehaviour {

    public bool addMeshCollider = false;
    public bool castShadow = true;
    public bool combineOnStart = true, destroyAfterOptimized = false;

    /// Usually rendering with triangle strips is faster.
    /// However when combining objects with very low triangle counts, it can be faster to use triangles.
    /// Best is to try out which value is faster in practice.
    public int frameToWait = 0;

    public bool generateTriangleStrips = true;
    public bool keepLayer = true;
    public bool receiveShadow = true;

    private void Start() {
        if (combineOnStart && frameToWait == 0) {
            Combine();
        } else {
            StartCoroutine(CombineLate());
        }
    }

    private IEnumerator CombineLate() {
        for (int i = 0; i < frameToWait; i++) {
            yield return 0;
        }
        Combine();
    }

    [ContextMenu("Combine Now on Childs")]
    public void CallCombineOnAllChilds() {
#if UNITY_EDITOR
        if (!Application.isPlaying) {
            Undo.RegisterSceneUndo("Combine meshes");
        }
#endif
        SS_CombineChildrenExtended[] c = gameObject.GetComponentsInChildren<SS_CombineChildrenExtended>();
        int count = c.Length;
        for (int i = 0; i < count; i++) {
            if (c[i] != this) {
                c[i].Combine();
            }
        }
        combineOnStart = enabled = false;
    }

    /// This option has a far longer preprocessing time at startup but leads to better runtime performance.
    [ContextMenu("Combine Now")]
    public void Combine() {
#if UNITY_EDITOR
        if (!Application.isPlaying) {
            Undo.RegisterSceneUndo("Combine meshes");
        }
#endif
        Component[] filters = GetComponentsInChildren(typeof(MeshFilter));
        Matrix4x4 myTransform = transform.worldToLocalMatrix;
        //var materialToMesh = new Hashtable();
        var materialToMesh = new Dictionary<Material, List<SS_MeshCombineUtility.MeshInstance>>();

        for (int i = 0; i < filters.Length; i++) {
            var filter = (MeshFilter) filters[i];
            Renderer curRenderer = filters[i].renderer;
            var instance = new SS_MeshCombineUtility.MeshInstance();
            instance.mesh = filter.sharedMesh;
            if (curRenderer != null && curRenderer.enabled && instance.mesh != null) {
                instance.transform = myTransform * filter.transform.localToWorldMatrix;

                Material[] materials = curRenderer.sharedMaterials;
                for (int m = 0; m < materials.Length; m++) {
                    instance.subMeshIndex = Math.Min(m, instance.mesh.subMeshCount - 1);

                    List<SS_MeshCombineUtility.MeshInstance> objects;
                    materialToMesh.TryGetValue(materials[m], out objects);
                    if (objects != null) {
                        objects.Add(instance);
                    } else {
                        objects = new List<SS_MeshCombineUtility.MeshInstance>();
                        objects.Add(instance);
                        materialToMesh.Add(materials[m], objects);
                    }
                }
                if (Application.isPlaying && destroyAfterOptimized && combineOnStart) {
                    Destroy(curRenderer.gameObject);
                } else if (destroyAfterOptimized) {
                    DestroyImmediate(curRenderer.gameObject);
                } else {
                    curRenderer.enabled = false;
                }
            }
        }

        foreach (KeyValuePair<Material, List<SS_MeshCombineUtility.MeshInstance>> de in materialToMesh) {
            var elements = de.Value;
            var instances = elements.ToArray();

            // We have a maximum of one material, so just attach the mesh to our own game object
            if (materialToMesh.Count == 1) {
                // Make sure we have a mesh filter & renderer
                if (GetComponent(typeof(MeshFilter)) == null) {
                    gameObject.AddComponent(typeof(MeshFilter));
                }
                if (!GetComponent("MeshRenderer")) {
                    gameObject.AddComponent("MeshRenderer");
                }

                var filter = (MeshFilter) GetComponent(typeof(MeshFilter));
                if (Application.isPlaying) {
                    filter.mesh = SS_MeshCombineUtility.Combine(instances, generateTriangleStrips);
                } else {
                    filter.sharedMesh = SS_MeshCombineUtility.Combine(instances, generateTriangleStrips);
                }
                renderer.material = (Material) de.Key;
                renderer.enabled = true;
                if (addMeshCollider) {
                    gameObject.AddComponent<MeshCollider>();
                }
                renderer.castShadows = castShadow;
                renderer.receiveShadows = receiveShadow;
            }
                // We have multiple materials to take care of, build one mesh / gameobject for each material
                // and parent it to this object
            else {
                var go = new GameObject("Combined mesh");
                if (keepLayer) {
                    go.layer = gameObject.layer;
                }
                go.transform.parent = transform;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localPosition = Vector3.zero;
                go.AddComponent(typeof(MeshFilter));
                go.AddComponent("MeshRenderer");
                go.renderer.material = (Material) de.Key;
                var filter = (MeshFilter) go.GetComponent(typeof(MeshFilter));
                if (Application.isPlaying) {
                    filter.mesh = SS_MeshCombineUtility.Combine(instances, generateTriangleStrips);
                } else {
                    filter.sharedMesh = SS_MeshCombineUtility.Combine(instances, generateTriangleStrips);
                }
                go.renderer.castShadows = castShadow;
                go.renderer.receiveShadows = receiveShadow;
                if (addMeshCollider) {
                    go.AddComponent<MeshCollider>();
                }
            }
        }
    }

    [ContextMenu("Save mesh as asset")]
    private void SaveMeshAsAsset() {
#if UNITY_EDITOR
        string path = EditorUtility.SaveFilePanelInProject("Save mesh asset", "CombinedMesh", "asset", "Select save file path");
        if (!string.IsNullOrEmpty(path)) {
            AssetDatabase.CreateAsset(GetComponent<MeshFilter>().sharedMesh, path);
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
        }
#endif
    }
}

#pragma warning restore 0618