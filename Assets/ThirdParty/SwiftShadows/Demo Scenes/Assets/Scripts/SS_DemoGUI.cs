using UnityEngine;

/// <summary>
///     Base GUI used for demos.
/// </summary>
public abstract class SS_DemoGUI : MonoBehaviour {
    public Texture2D Logo;
    protected bool visible = true;

    protected virtual void OnLevelWasLoaded(int level) {
        Application.targetFrameRate = 10000;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SS_CameraFade.StartAlphaFade(Color.black, true, 0.5f, 0.5f);
    }

    protected virtual void OnDestroy() {
       Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    protected virtual void Start() {
        useGUILayout = false;
    }

    protected virtual void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SS_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("SS_Menu"));
        }

        if (Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.Return)) {
            visible = !visible;
        }
    }
}