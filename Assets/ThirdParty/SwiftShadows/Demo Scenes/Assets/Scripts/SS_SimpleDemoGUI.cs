using UnityEngine;
using LostPolygon.Internal.SwiftShadows;

#if !UNITY_3_5
using LostPolygon.SwiftShadows;
#endif

/// <summary>
/// GUI used for demos. 
/// </summary>
public class SS_SimpleDemoGUI : SS_DemoGUI {
    public Camera TerrainDemoPrimaryCamera;
    public LayerMask TerrainDemoPrimaryCameraLayers;
    public Camera TerrainDemoSecondaryCamera;
    public GameObject TerrainDemoMobileControls;

    private bool _terrainDemoSecondaryCameraOn = true;
    private int _terrainDemoPrimaryCameraLayersDefault;

    protected override void Start() {
        base.Start();

        Application.targetFrameRate = 2000;
        if (SS_GUILayout.IsRuntimePlatformMobile())
            Shader.globalMaximumLOD = 150;

        if (TerrainDemoPrimaryCamera != null)
            _terrainDemoPrimaryCameraLayersDefault = TerrainDemoPrimaryCamera.cullingMask;
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        Shader.globalMaximumLOD = 1000;
    }

    private void OnGUI() {
        if (!visible) {
            return;
        }

        const float initWidth = 275f;
        float initHeight = 70f;
        const float initItemHeight = 30f;

        SS_GUILayout.itemWidth = initWidth - 20f;
        SS_GUILayout.itemHeight = initItemHeight;
        SS_GUILayout.yPos = 0f;
        SS_GUILayout.hovered = false;

        if (SS_GUILayout.IsRuntimePlatformMobile()) {
            SS_GUILayout.UpdateScaleMobile();
        } else {
            SS_GUILayout.UpdateScaleDesktop(initHeight + 30f);
        }

        string levelName = Application.loadedLevelName;
        string sceneName = string.Empty;
        string text = string.Empty;
        switch (levelName) {
            case "SS_DiscoLights":
                sceneName = "Disco Lights Scene";
                text = "    This is an example of a more creative use of Swift Shadows, with hundreds " +
                       "of shadow objects calculated and drawn at the same. All light spots " +
                       "use a special \"light\" material and are rendered within a single draw call.";
                break;
            case "SS_Helicopter":
                sceneName = "Animated Shadows Scene";
                text = "    This scene demonstrates shadows following the transform of object they " +
                            "are attached to. In this scene, the blades and body of helicopter, as " +
                            "well as the platform and dish of the radar station, are actually two " +
                            "different overlayed shadows. Nevertheless, the shadows are still drawn in a single " +
                            "draw call by using a texture atlas.";
                break;
            case "SS_MovingCubes":
                sceneName = "Moving Cubes Scene";
                text = "    This is a generic demonstration of what Swift Shadows can do.\n" +
                       "    A raycast from the light source to the cubes determines the position of shadow, " +
                       "and a projected quad is rendered in place. All shadows are drawn with a single " +
                       "draw call, which is especially useful for mobile devices.\n" +
                       "    You can also notice the limitation: shadows are casted on one surface at time and " +
                       "can extend beyond the surface they are projected on. Shadows can be set to fade away " +
                       "as they fall at extreme angles to make this effect less noticeable.";
                break;
            case "SS_MultipleShadows":
                sceneName = "Multiple Shadows Demo";
                text = "    Multiple shadows components can be set for a single object. " +
                       "There are 4 light sources in this scene, each resulting in a separate shadow of the object.";
                break;
            case "SS_TerrainInteraction":
                initHeight += 25f;
                sceneName = "Terrain Interaction Demo";
                text = "    This demonstrates how Swift Shadows can be used on uneven surfaces like terrain.\n" +
                       "    \"Use two cameras\" enables drawing the character and shadow on a separate pass, " +
                       "which eliminates the effect of shadow being clipped with terrain. This comes with the cost " +
                       "of some possible artifacts that aren't noticeable for most situations.\n";
                break;
        }

        if (levelName != "SS_TerrainInteraction") {
            if (SS_GUILayout.IsRuntimePlatformMobile()) {
                text += "\n    You can rotate the camera with two fingers.\n";
            } else {
                if (levelName == "SS_DiscoLights") {
                    text += "\n    You can rotate the camera by holding the right mouse button.\n";
                } else {
                    text += "\n    You can rotate the camera by holding the right mouse button and zoom with mouse wheel.\n";
                }
            }
        }


        foreach (SS_ShadowMeshManager meshManager in SS_ShadowManager.Instance.ShadowManagers) {
            text += string.Format((meshManager.IsStatic ? "\nTotal shadows (static): " : "\nTotal shadows: ") + "{0}, drawn: {1}", 
                meshManager.ShadowsCount,
                meshManager.VisibleShadowsCount);
        }

        var centeredStyle = GUI.skin.label;
        centeredStyle.alignment = TextAnchor.MiddleLeft;

        float textHeight = centeredStyle.CalcHeight(new GUIContent(text, ""), SS_GUILayout.itemWidth);
        initHeight += textHeight;

        GUI.BeginGroup(new Rect(15f, 15f, initWidth, initHeight), sceneName, GUI.skin.window);

        SS_GUILayout.yPos = 20f;

        SS_GUILayout.itemHeight = centeredStyle.CalcHeight(new GUIContent(text, ""), SS_GUILayout.itemWidth);

        SS_GUILayout.Label(text);
        SS_GUILayout.itemHeight = initItemHeight;

        if (levelName == "SS_TerrainInteraction" && TerrainDemoPrimaryCamera != null) {
            bool changed;
            _terrainDemoSecondaryCameraOn = SS_GUILayout.Toggle(_terrainDemoSecondaryCameraOn, "Use two cameras", out changed);
            if (changed) {
                if (_terrainDemoSecondaryCameraOn) {
                    SS_Extensions.SetActive(TerrainDemoSecondaryCamera.gameObject, true);
                    TerrainDemoPrimaryCamera.cullingMask = _terrainDemoPrimaryCameraLayersDefault;
                } else {
                    SS_Extensions.SetActive(TerrainDemoSecondaryCamera.gameObject, false);
                    TerrainDemoPrimaryCamera.cullingMask = TerrainDemoPrimaryCameraLayers;
                }

                SS_ShadowManager.Instance.UpdateCameraEvents(TerrainDemoSecondaryCamera);
            }
        }

        GUI.color = new Color(1f, 0.6f, 0.6f, 1f);
        if (GUI.Button(new Rect(SS_GUILayout.paddingLeft, initHeight - 40f, SS_GUILayout.itemWidth, 30f), "Back to Main Menu")) {
            SS_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("SS_Menu"));
        }

        GUI.EndGroup();

        GUI.color = Color.white;
        SS_GUILayout.DrawLogo(Logo);
    }
}