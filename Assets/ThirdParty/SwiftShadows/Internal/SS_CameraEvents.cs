using UnityEngine;

#if !UNITY_3_5
namespace LostPolygon.SwiftShadows {
#endif
/// <summary>
/// Submits rendering the shadows for camera this script is attached to
/// </summary>
public class SS_CameraEvents : MonoBehaviour {
    private Camera _camera;

    private void Start() {
        _camera = gameObject.GetComponent<Camera>();
    }

    /// <summary>
    /// Called right before camera is going to cull the scene.
    /// </summary>
    private void OnPreCull() {
        if (!SS_ShadowManager.IsDestroyed) // Only render when there is manager available
            SS_ShadowManager.Instance.OnCameraPreCull(_camera);
    }
}
#if !UNITY_3_5
}
#endif