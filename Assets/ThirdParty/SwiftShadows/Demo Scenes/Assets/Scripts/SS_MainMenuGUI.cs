using UnityEngine;

public class SS_MainMenuGUI : MonoBehaviour {
    public Texture2D Logo;

    private void OnLevelWasLoaded(int level) {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        SS_CameraFade.StartAlphaFade(Color.black, true, 0.5f, 0.5f);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    private void OnGUI() {
        var centeredStyle = GUI.skin.label;

        const float width = 250f;
        const float buttonHeight = 35f;

        float height = 155f + buttonHeight * 3f;
        float logoHeight = Logo.height;
        
        if (Application.isWebPlayer) {
            height -= 25f + buttonHeight;
        }

        float totalHeight = height + logoHeight;
        if (SS_GUILayout.IsRuntimePlatformMobile()) {
            SS_GUILayout.UpdateScaleMobile();
        }
        else {
            SS_GUILayout.UpdateScaleDesktop(totalHeight);
        }

        Rect totalRect = new Rect(
            Screen.width / 2f / SS_GUILayout.scaleFactor - width / 2f,
            Screen.height / 2f / SS_GUILayout.scaleFactor - totalHeight / 2f,
            width,
            totalHeight
            );

        Rect logoRect = totalRect;
        logoRect.height = logoHeight;

        Rect rect = totalRect;
        rect.yMin += logoHeight;

        GUILayout.BeginArea(
            logoRect,
            "", ""
            );
        GUILayout.Label(Logo);
        GUILayout.EndArea();

        GUILayout.BeginArea(
            rect,
            "", 
            GUI.skin.textArea
            );
        GUILayout.BeginVertical();

        if (GUILayout.Button("Moving cubes", GUILayout.Height(buttonHeight))) {
            SS_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("SS_MovingCubes"));
        }
        if (GUILayout.Button("Animated shadows", GUILayout.Height(buttonHeight))) {
            SS_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("SS_Helicopter"));
        }

        if (GUILayout.Button("Terrain interaction", GUILayout.Height(buttonHeight))) {
            SS_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("SS_TerrainInteraction"));
        }

        if (GUILayout.Button("Disco lights", GUILayout.Height(buttonHeight))) {
            SS_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("SS_DiscoLights"));
        }
        if (GUILayout.Button("Multiple shadows", GUILayout.Height(buttonHeight))) {
            SS_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("SS_MultipleShadows"));
        }

        if (!Application.isWebPlayer) {
            GUILayout.Space(20);
            GUI.color = new Color(1f, 0.6f, 0.6f, 1f);
            if (GUILayout.Button("Quit", GUILayout.Height(buttonHeight))) {
                SS_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, Application.Quit);
            }
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUI.color = Color.black;
        centeredStyle.alignment = TextAnchor.UpperLeft;

        if (!SS_GUILayout.IsRuntimePlatformMobile() && !Application.isEditor)
        {
            Screen.fullScreen = GUI.Toggle(
                new Rect(rect.xMin - 90f,
                    rect.yMin,
                         100f, 400f), Screen.fullScreen, " Fullscreen");
        }
        //GUI.Label(new Rect(rect.xMax + 10f,
        //    rect.yMin, Mathf.Min(200f, Screen.width - rect.xMax - 10f), 400f),
        //          "You can press Enter/Menu button to disable GUI (as Unity GUI may cause slowdown, especially on mobile)");
    }
}