using UnityEngine;
using LostPolygon.Internal.SwiftShadows;

public class SS_ThirdPersonControllerJoystick : MonoBehaviour {
    public SS_ThirdPersonController ThirdPersonController;
    public SS_Joystick MoveJoystick;

    private void Start() {
        if (!SS_GUILayout.IsRuntimePlatformMobile()) {
            SS_Extensions.SetActive(gameObject, false);
        }
    }

    private void Update() {
        if (MoveJoystick != null) {
            ThirdPersonController.UseExternalInput = true;
            ThirdPersonController.ExternalInput = MoveJoystick.position;
        }
    }
}

