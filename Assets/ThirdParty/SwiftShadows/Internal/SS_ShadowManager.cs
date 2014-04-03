using System;
using System.Collections.Generic;
using UnityEngine;
using LostPolygon.Internal.SwiftShadows;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if !UNITY_3_5
using LostPolygon.SwiftShadows;
#endif

/// <summary>
/// Main shadow manager class. Controls the rendering, triggers the calculation, creates and manages mesh managers.
/// </summary>
public class SS_ShadowManager : MonoBehaviour {
    private const string kGameObjectName = "_SwiftShadowManager";

    /// <summary>
    /// An instance of the singleton.
    /// </summary>
    private static SS_ShadowManager _instance;

    /// <summary>
    /// A value indicating whether an instance of SS_ShadowManager is instantiated
    /// </summary>
    private static bool _isDestroyed = true;

#if UNITY_EDITOR
    /// <summary>
    /// Used to determine whether the assembly reload was triggered.
    /// </summary>
    private RecompiledMarker _recompileMarker;
#endif

    /// <summary>
    /// Dictonary of shadows mesh managers. The key is a numeric value, unique for a combination of IsStatic, Material, and Layer.
    /// </summary>
    private readonly Dictionary<int, SS_ShadowMeshManager> _meshManagers = new Dictionary<int, SS_ShadowMeshManager>();

    /// <summary>
    /// The list of shadows mesh managers that were added mid-calculatiom.
    /// </summary>
    private readonly List<SS_ShadowMeshManager> _meshManagersNew = new List<SS_ShadowMeshManager>();
    private SS_ShadowMeshManager[] _meshManagersArrayCache = new SS_ShadowMeshManager[4];
    private Plane[] _cameraFrustumPlanes = new Plane[6];

    /// <summary>
    /// Whether the shadow recalculation is going on right now.
    /// </summary>
    private bool _isRecalculatingMesh;

    /// <summary>
    /// Prevents a default instance of the <see cref="SS_ShadowManager"/> class from being created.
    /// </summary>
    private SS_ShadowManager() {
    }

    /// <summary>
    ///     Gets a value indicating whether an instance of SS_ShadowManager is instantiated.
    /// </summary>
    public static bool IsDestroyed {
        get {
            return _isDestroyed;
        }
    }

    /// <summary>
    /// Gets the instance of SS_ShadowManager, creating one if needed.
    /// </summary>
    public static SS_ShadowManager Instance {
        get {
            if (_instance == null) {
                // Trying to find an existing instance in the scene
                _instance = FindObjectOfType(typeof(SS_ShadowManager)) as SS_ShadowManager;

                // Creating a new instance in case there are no instances present in the scene
                if (_instance == null) {
                    GameObject go = new GameObject(kGameObjectName);
                    go.hideFlags = HideFlags.NotEditable;
                    _instance = go.AddComponent<SS_ShadowManager>();
                }
            }

            return _instance;
        }
    }

    /// <summary>
    /// Gets the list of shadow mesh managers.
    /// </summary>
    public ICollection<SS_ShadowMeshManager> ShadowManagers {
        get {
            return _meshManagers.Values;
        }
    }

    /// <summary>
    /// Attaches SS_CameraEvents to every enabled cameras, if needed.
    /// </summary>
    public void UpdateCameraEvents() {
        UpdateCameraEvents(Camera.allCameras);
    }

    /// <summary>
    /// Attaches SS_CameraEvents to the camera.
    /// </summary>
    /// <param name="camera">
    /// The camera to attach the SS_CameraEvents component.
    /// </param>
    public void UpdateCameraEvents(Camera camera) {
        if (camera.GetComponent<SS_CameraEvents>() == null) {
            camera.gameObject.AddComponent<SS_CameraEvents>();
        }
    }

    /// <summary>
    /// Attaches SS_CameraEvents to the array of cameras.
    /// </summary>
    /// <param name="cameras">
    /// The cameras to attach the SS_CameraEvents component.
    /// </param>
    public void UpdateCameraEvents(Camera[] cameras) {
        for (int i = 0; i < cameras.Length; i++) {
            if (cameras[i].GetComponent<SS_CameraEvents>() == null) {
                cameras[i].gameObject.AddComponent<SS_CameraEvents>();
            }
        }
    }

    /// <summary>
    /// Forces the recalculation of static shadows.
    /// </summary>
    public void UpdateStaticShadows() {
#if !UNITY_FLASH
        foreach (SS_ShadowMeshManager meshManager in _meshManagers.Values) {
            if (meshManager.IsStatic) {
                meshManager.ForceStaticRecalculate();
            }
        }

#else
        foreach (KeyValuePair<int, SS_ShadowMeshManager> meshManager in _meshManagers) {
            if (meshManager.Value.IsStatic)
                meshManager.Value.ForceStaticRecalculate();
        }
#endif
    }

    /// <summary>
    /// Registers the shadow in the system.
    /// </summary>
    /// <param name="shadow">
    /// The shadow to register.
    /// </param>
    public void RegisterShadow(SwiftShadow shadow) {
        GetMeshManager(shadow).RegisterShadow(shadow);
    }

    /// <summary>
    /// Unregisters the shadow in the system.
    /// </summary>
    /// <param name="shadow">
    /// The shadow to unregister.
    /// </param>
    public void UnregisterShadow(SwiftShadow shadow) {
        SS_ShadowMeshManager meshManager;
        bool isExists = _meshManagers.TryGetValue(shadow.GetMeshManagerHashCode(), out meshManager);

        if (isExists) {
            meshManager.UnregisterShadow(shadow);
        }
    }

    /// <summary>
    /// Called by camera on OnPreCull event.
    /// </summary>
    /// <param name="camera">
    /// The camera to render the shadows.
    /// </param>
    public void OnCameraPreCull(Camera camera) {
        Render(camera);
    }

#if UNITY_EDITOR
    private void LateUpdate() {
        if (_recompileMarker == null) {
            Initialize();
            _recompileMarker = new RecompiledMarker();
        }

        UpdateCameraEvents(SceneView.GetAllSceneCameras());
    }
#endif

    private void Start() {
        // Kill other instances
        if (FindObjectsOfType(typeof(SS_ShadowManager)).Length > 1) {
            DestroyImmediate(gameObject);
            Debug.LogWarning("Multiple " + kGameObjectName + " instances found, destroying");
            return;
        }

        Initialize();
    }

    private void OnDestroy() {
        _instance = null;
        _isDestroyed = true;
    }

    private void OnEnable() {
#if UNITY_EDITOR
        if (_recompileMarker == null) {
            Initialize();
            _recompileMarker = new RecompiledMarker();
        }
#endif
    }

    private void OnDisable() {
#if !UNITY_FLASH
        foreach (SS_ShadowMeshManager meshManager in _meshManagers.Values) {
            meshManager.FreeMesh();
        }

#else
        foreach (KeyValuePair<int, SS_ShadowMeshManager> meshManager in _meshManagers) {
            meshManager.Value.FreeMesh();
        }
#endif
        _meshManagers.Clear();
        _meshManagersNew.Clear();
        _meshManagersArrayCache = null;
    }

    /// <summary>
    /// Sets up the manager.
    /// </summary>
    private void Initialize() {
        _instance = this;
#if UNITY_EDITOR
        _recompileMarker = new RecompiledMarker();
#endif
        _isDestroyed = false;
        UpdateCameraEvents(Camera.allCameras);
    }

    /// <summary>
    /// Renders the shadows to a camera
    /// </summary>
    /// <param name="camera">
    /// The camera to render shadows on.
    /// </param>
    private void Render(Camera camera) {
        // Calculate the camera frustum planes
        if (camera != null) {
#if !UNITY_FLASH && !UNITY_IPHONE && !UNITY_WINRT && !UNITY_WEBPLAYER
            GeometryUtilityInternal.CalculateFrustumPlanes(_cameraFrustumPlanes, camera);
#else
            _cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            #endif
        }

        // Recreate the temporary array if it is too short
        int meshManagersCount = _meshManagers.Count;
        if (_meshManagersArrayCache.Length < meshManagersCount) {
            _meshManagersArrayCache = new SS_ShadowMeshManager[meshManagersCount];
        }

        // Copy the values into temporary array
        int meshManagerCounter = 0;
        foreach (var meshManager in _meshManagers) {
            _meshManagersArrayCache[meshManagerCounter] = meshManager.Value;
            meshManagerCounter++;
        }

        _isRecalculatingMesh = true;

        // Clearing list of managers added on previous recalculations
        _meshManagersNew.Clear();

        // Loop known mesh managers, we don't care if new items will be added inside the loop
        for (int i = 0; i < meshManagersCount; i++) {
            _meshManagersArrayCache[i].RecalculateGeometry(_cameraFrustumPlanes);
            _meshManagersArrayCache[i].DrawMesh(camera);
        }

        _isRecalculatingMesh = false;

        // Calculate & draw the newly added managers (if AutoStatic is used)
        for (int i = 0; i < _meshManagersNew.Count; i++) {
            _meshManagersNew[i].RecalculateGeometry(_cameraFrustumPlanes);
            _meshManagersNew[i].DrawMesh(camera);
        }
    }

    /// <summary>
    /// Returns the mesh managers suitable for a shadow.
    /// </summary>
    /// <param name="shadow">
    /// The shadow to get mesh manager for.
    /// </param>
    /// <returns>
    /// The <see cref="SS_ShadowMeshManager"/>.
    /// </returns>
    private SS_ShadowMeshManager GetMeshManager(SwiftShadow shadow) {
        SS_ShadowMeshManager meshManager;
        bool isExists = _meshManagers.TryGetValue(shadow.GetMeshManagerHashCode(), out meshManager);
        if (isExists) {
            return meshManager;
        }

        if (shadow.Material == null)
            throw new ArgumentNullException("shadow.Material");

        meshManager = new SS_ShadowMeshManager(shadow.Material, shadow.Layer, shadow.IsStatic);
        _meshManagers.Add(meshManager.GetInstanceHashCode(), meshManager);
        if (_isRecalculatingMesh) {
            _meshManagersNew.Add(meshManager);
        }

        return meshManager;
    }
}