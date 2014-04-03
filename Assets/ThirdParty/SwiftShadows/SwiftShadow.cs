using UnityEngine;
using LostPolygon.Internal.SwiftShadows;

/// <summary>
/// Main Swift Shadows component.
/// </summary>
[AddComponentMenu("Lost Polygon/Swift Shadow")]
public class SwiftShadow : MonoBehaviour {
    /// <summary>
    /// Enum representing the source of light direction vector.
    /// </summary>
    public enum LightVectorSourceEnum {
        StaticVector, 
        GameObject
    }

    /// <summary>
    /// The result of RecalculateShadow().
    /// </summary>
    public enum RecalculateShadowResult {
        /// <summary>
        /// The shadow was recalculated successfully.
        /// </summary>
        Recalculated, 

        /// <summary>
        /// The shadow recalculation was skipped.
        /// </summary>
        Skipped, 

        /// <summary>
        /// The shadow moved to another mesh manager.
        /// </summary>
        ChangedManager
    }


    /// <summary>
    /// Shadow projection vertices.
    /// </summary>
    private readonly Vector3[] _baseVertices = new Vector3[4];

    /// <summary>
    /// Vertices of shadow quad.
    /// </summary>
    private readonly Vector3[] _shadowVertices = new Vector3[4];

    /// <summary>
    /// Texture UV coordinates.
    /// </summary>
    private readonly Vector2[] _textureUV = new Vector2[4];

    /// <summary>
    /// Amount of time passed since last going to non-static state.
    /// </summary>
    private float _autoStaticTimeCounter;

    /// <summary>
    /// Skipped frames counter.
    /// </summary>
    private int _frameSkipCounter;

    /// <summary>
    /// The initial alpha of shadow.
    /// </summary>
    private float _initialAlpha;

    /// <summary>
    /// Whether the shadows is destroyed already.
    /// </summary>
    private bool _isDestroyed;

    /// <summary>
    /// Whether this is the first calculation of the shadows.
    /// </summary>
    private bool _isFirstCalculation;

    /// <summary>
    /// Whether the owner GameObject is active.
    /// </summary>
    private bool _isGameObjectActivePrev;

    /// <summary>
    /// Whether the shadows is initialized.
    /// </summary>
    private bool _isInitialized;

    /// <summary>
    /// Whether the shadow is visible at the last recalculation.
    /// </summary>
    private bool _isVisible;

    /// <summary>
    /// Current surface normal
    /// </summary>
    private Vector3 _normal;

    /// <summary>
    /// Cached object transform.
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Previous value of transform.forward.
    /// </summary>
    private Vector3 _transformForwardPrev;

    /// <summary>
    /// Previous value of transform.position.
    /// </summary>
    private Vector3 _transformPositionPrev;

    /// <summary>
    /// Cached color in a more fast format.
    /// </summary>
    private Color32 _color32 = new Color32(0, 0, 0, 255);

    /// <summary>
    /// Whether the Light, attached to the LightSourceObject,
    /// is directional.
    /// </summary>
    private bool _lightSourceObjectIsDirectionalLight;

    /// <summary>
    /// Last calculated light direction vector.
    /// </summary>
    private Vector3 _actualLightVectorPrev;

    /// <summary>
    /// The version of this component. Could be used in the future to manage updates.
    /// </summary>
#pragma warning disable 0414
    private int _componentVersion = 1;
#pragma warning restore 0414

    #region Properties backing fields
    [SerializeField]
    private Color _color = new Color(0f, 0f, 0f, 1f);

    [SerializeField]
    private float _angleFadeMax = 70f;

    [SerializeField]
    private float _angleFadeMin = 60f;

    [SerializeField]
    private float _aspectRatio = 1f;

    [SerializeField]
    private float _autoStaticTime;

    [SerializeField]
    private float _fadeDistance = 5f;

    [SerializeField]
    private int _forceLayer;

    [SerializeField]
    private int _frameSkip;

    [SerializeField]
    private Vector3 _lightVector = new Vector3(0f, -1f, 0f);

    [SerializeField]
    private LightVectorSourceEnum _lightVectorSource = LightVectorSourceEnum.StaticVector;

    [SerializeField]
    private Transform _lightSourceObject;

    [SerializeField]
    private float _projectionDistance = 20f;

    [SerializeField]
    private float _shadowOffset = 0.01f;

    [SerializeField]
    private float _shadowSize = 1f;

    [SerializeField]
    private Material _material;

    [SerializeField]
    private Rect _textureUVRect = new Rect(0f, 0f, 1f, 1f);

    [SerializeField]
    private LayerMask _layerMask = unchecked(~0);

    [SerializeField]
    private bool _useForceLayer;

    [SerializeField]
    private bool _isPerspectiveProjection;

    [SerializeField]
    private bool _cullInvisible = true;

    [SerializeField]
    private bool _isStatic;
    #endregion

    #region Properties
    public bool IsVisible {
        get {
            return _isVisible;
        }
    }

    public Vector3 Normal {
        get {
            return _normal;
        }
    }

    public Color32 Color32 {
        get {
            return _color32;
        }

        set {
            Color = value;
        }
    }

    public Color Color {
        get {
            return _color;
        }

        set {
            _color = value;
            _color32 = value;
            _initialAlpha = _color.a;

            if (Application.isPlaying && _material != null) {
                _material.color = value;
            }
        }
    }

    public Vector2[] TextureUV {
        get {
            return _textureUV;
        }
    }

    public Rect TextureUVRect {
        get {
            return _textureUVRect;
        }

        set {
            value.xMin = Mathf.Clamp(value.xMin, 0f, 1f);
            value.yMin = Mathf.Clamp(value.yMin, 0f, 1f);
            value.xMax = Mathf.Clamp(value.xMax, 0f, 1f);
            value.yMax = Mathf.Clamp(value.yMax, 0f, 1f);
            _textureUVRect = value;
            SetTextureUV(value);
        }
    }

    public Material Material {
        get {
            return _material;
        }

        set {
            if (_material != value) {
                UnregisterShadow();
                _material = value;
                RegisterShadow();
            }
        }
    }

    public LayerMask LayerMask {
        get {
            return _layerMask;
        }

        set {
            if (_layerMask != value) {
                UnregisterShadow();
                _layerMask = value;
                RegisterShadow();
            }
        }
    }

    public bool IsStatic {
        get {
            return _isStatic;
        }

        set {
            if (_isStatic != value) {
                UnregisterShadow();
                _isStatic = value;
                RegisterShadow();
            }
        }
    }

    public LightVectorSourceEnum LightVectorSource {
        get {
            return _lightVectorSource;
        }

        set {
            _lightVectorSource = value;
        }
    }

    public Vector3 LightVector {
        get {
            return _lightVector;
        }

        set {
            _lightVector = value.normalized;
        }
    }

    public Transform LightSourceObject {
        get {
            return _lightSourceObject;
        }

        set {
            if (_lightSourceObject != value) {
                _lightSourceObject = value;

                UpdateDirectionalLight();
            }
        }
    }

    public float ShadowSize {
        get {
            return _shadowSize;
        }

        set {
            _shadowSize = Mathf.Max(0f, value);
        }
    }

    public float ShadowOffset {
        get {
            return _shadowOffset;
        }

        set {
            _shadowOffset = Mathf.Max(0f, value);
        }
    }

    public float ProjectionDistance {
        get {
            return _projectionDistance;
        }

        set {
            _projectionDistance = Mathf.Max(0f, value);
            _fadeDistance = Mathf.Clamp(value, 0f, _projectionDistance);
        }
    }

    public float FadeDistance {
        get {
            return _fadeDistance;
        }

        set {
            _fadeDistance = Mathf.Clamp(value, 0f, _projectionDistance);
        }
    }

    public bool IsPerspectiveProjection {
        get {
            return _isPerspectiveProjection;
        }

        set {
            _isPerspectiveProjection = value;
        }
    }

    public int FrameSkip {
        get {
            return _frameSkip;
        }

        set {
            _frameSkip = Mathf.Clamp(value, 0, 50);
        }
    }

    public float AutoStaticTime {
        get {
            return _autoStaticTime;
        }

        set {
            if (_autoStaticTime != value) {
                _autoStaticTime = Mathf.Max(0f, value);
                _autoStaticTimeCounter = 0;
            }
        }
    }

    public float AngleFadeMin {
        get {
            return _angleFadeMin;
        }

        set {
            _angleFadeMin = Mathf.Clamp(value, 0f, 90f);
        }
    }

    public float AngleFadeMax {
        get {
            return _angleFadeMax;
        }

        set {
            _angleFadeMax = Mathf.Clamp(value, 0f, 90f);
        }
    }

    public float AspectRatio {
        get {
            return _aspectRatio;
        }

        set {
            _aspectRatio = Mathf.Max(0f, value);
        }
    }

    public Vector3[] ShadowVertices {
        get {
            return _shadowVertices;
        }
    }

    public bool CullInvisible {
        get {
            return _cullInvisible;
        }

        set {
            _cullInvisible = value;
        }
    }

    public int Layer {
        get {
            return _useForceLayer ? _forceLayer : gameObject.layer;
        }
    }

    public bool UseForceLayer {
        get {
            return _useForceLayer;
        }

        set {
            if (_useForceLayer != value) {
                UnregisterShadow();
                _useForceLayer = value;
                RegisterShadow();
            }
        }
    } 

    public int ForceLayer {
        get {
            return _forceLayer;
        }

        set {
            if (_forceLayer != value) {
                UnregisterShadow();
                _forceLayer = value;
                RegisterShadow();
            }
        }
    }
    #endregion

    /// <summary>
    ///     Registers the shadow in the manager.
    /// </summary>
    public void RegisterShadow() {
        if (!Application.isPlaying) {
            return;
        }

        SS_ShadowManager.Instance.RegisterShadow(this);
    }

    /// <summary>
    ///     Unregister the shadow from the manager.
    /// </summary>
    public void UnregisterShadow() {
        if (!SS_ShadowManager.IsDestroyed && Application.isPlaying) {
            SS_ShadowManager.Instance.UnregisterShadow(this);
        }
    }

    public RecalculateShadowResult RecalculateShadow(Plane[] frustumPlanes, bool force) {
        _isVisible = _isStatic;

        // Determine whether the owner GameObject changed the active state
        // and react correspondingly
        bool isGameObjectActive = SS_Extensions.IsActiveInHierarchy(gameObject);
        if (isGameObjectActive != _isGameObjectActivePrev) {
            _isGameObjectActivePrev = isGameObjectActive;
            if (isGameObjectActive) {
                RegisterShadow();
                return RecalculateShadowResult.ChangedManager;
            } else {
                UnregisterShadow();
                return RecalculateShadowResult.ChangedManager;
            }
        }

        _isGameObjectActivePrev = isGameObjectActive;
        if (!isGameObjectActive)
            return RecalculateShadowResult.Skipped;

        // Updating the transform state (position and forward vectors)
        // Determine whether the transform has moved
        Vector3 transformPosition = _transform.position;
        Vector3 transformForward = _transform.forward;

        bool transformChanged = false;
        if (_autoStaticTime > 0) {
            if (transformPosition.x != _transformPositionPrev.x ||
                transformPosition.y != _transformPositionPrev.y ||
                transformPosition.z != _transformPositionPrev.z ||
                transformForward.x != _transformForwardPrev.x ||
                transformForward.y != _transformForwardPrev.y ||
                transformForward.z != _transformForwardPrev.z
                ) {
                _autoStaticTimeCounter = 0f;
                transformChanged = true;
            }

            _transformPositionPrev = transformPosition;
            _transformForwardPrev = transformForward;
        }

        if (!_isFirstCalculation) {
            // If we have AutoStatic 
            if (_autoStaticTime > 0f) {
                // If the object has moved - remove the shadow
                // from static manager and move to non-static
                if (_isStatic && transformChanged) {
                    UnregisterShadow();
                    _isStatic = false;
                    RegisterShadow();
                    return RecalculateShadowResult.ChangedManager;
                }

                // If the object hasn't moved for AutoStaticTime seconds,
                // then mark it as static
                _autoStaticTimeCounter += Time.deltaTime;
                if (!_isStatic && _autoStaticTimeCounter > _autoStaticTime) {
                    UnregisterShadow();
                    _isStatic = true;
                    RegisterShadow();
                    return RecalculateShadowResult.ChangedManager;
                }
            }

            // Do not update static shadows by default
            if (_isStatic && !force) {
                return RecalculateShadowResult.Skipped;
            }

            // Return if the time hasn't come yet
            if (_frameSkip != 0) {
                if (_frameSkipCounter < _frameSkip) {
                    _frameSkipCounter++;
                    return RecalculateShadowResult.Skipped;
                }

                _frameSkipCounter = 0;
            }
        }

        // Is this our first update?
        _isFirstCalculation = false;

        // Determine the light source position
        bool useLightSource = _lightVectorSource == LightVectorSourceEnum.GameObject && _lightSourceObject != null;
        Vector3 lightSourcePosition = useLightSource ? _lightSourceObject.transform.position : new Vector3();

        // The actual light direction vector that'll be used
        Vector3 actualLightVector;
        if (_lightVectorSource == LightVectorSourceEnum.GameObject && _lightSourceObject != null) {
            if (_lightSourceObjectIsDirectionalLight) {
                actualLightVector = _lightSourceObject.rotation * Vector3.forward;
            } else {
                actualLightVector.x = transformPosition.x - lightSourcePosition.x;
                actualLightVector.y = transformPosition.y - lightSourcePosition.y;
                actualLightVector.z = transformPosition.z - lightSourcePosition.z;
                actualLightVector = actualLightVector.FastNormalized();
            }
        } else {
            actualLightVector = _lightVector;
        }

        _actualLightVectorPrev = actualLightVector;

        // Do a raycast from transform.position to the center of the shadow
        RaycastHit hitInfo;
        bool raycastResult = Physics.Raycast(transformPosition, actualLightVector, out hitInfo, _projectionDistance, _layerMask);

        if (raycastResult) {
            // Scale the shadow respectively
            Vector3 lossyScale = transform.lossyScale;
            float scaledDoubleShadowSize = Mathf.Max(Mathf.Max(lossyScale.x, lossyScale.y), lossyScale.z) * ShadowSize;

            if (!_isStatic && _cullInvisible) {
                // We can calculate approximate bounds for orthographic shadows easily
                // and cull shadows based on these bounds and camera frustum
                if (!_isPerspectiveProjection) {
                    Bounds bounds = new Bounds(hitInfo.point, new Vector3(scaledDoubleShadowSize, scaledDoubleShadowSize, scaledDoubleShadowSize));
                    _isVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
                    if (!_isVisible) {
                        return RecalculateShadowResult.Skipped;
                    }
                } else {
                    // For perspective shadows, we can at least try to 
                    // not draw shadows that fall on invisible objects
                    Transform hitTransform = hitInfo.collider.transform;
                    if (frustumPlanes != null) {
                        Renderer hitRenderer = hitTransform != null ? hitTransform.renderer : null;
                        if (hitRenderer != null) {
                            _isVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, hitRenderer.bounds);
                            if (!_isVisible) {
                                return RecalculateShadowResult.Skipped;
                            }
                        }
                    }
                }
            }

            // Calculate angle from light direction vector to surface normal
            _normal = hitInfo.normal;
            float angleToNormal = SS_Math.FastAcos(-Vector3.Dot(actualLightVector, _normal)) * Mathf.Rad2Deg;
            if (angleToNormal > _angleFadeMax) {
                // Skip shadows that fall with extreme angles
                _isVisible = false;
                return RecalculateShadowResult.Skipped;
            }

            // Determine the forward direction of shadow base quad
            Vector3 forward;
            float dot = Vector3.Dot(transformForward, actualLightVector);
            if (Mathf.Abs(dot) < 1f - Vector3.kEpsilon) {
                forward = (transformForward - dot * actualLightVector).FastNormalized();
            } else {
                // If the forward direction matches the light direction vector somehow
                Vector3 transformUp = _transform.up;
                forward = (transformUp - Vector3.Dot(transformUp, actualLightVector) * actualLightVector).FastNormalized();
            }

            // Rotation of shadow base quad
            Quaternion rotation = Quaternion.LookRotation(forward, -actualLightVector);
            
            // Optimized version of
            // Vector3 right = rotation * Vector3.right;
            float num2 = rotation.y * 2f;
            float num3 = rotation.z * 2f;
            float num5 = rotation.y * num2;
            float num6 = rotation.z * num3;
            float num7 = rotation.x * num2;
            float num8 = rotation.x * num3;
            float num11 = rotation.w * num2;
            float num12 = rotation.w * num3;
            Vector3 right;
            right.x = 1f - (num5 + num6);
            right.y = num7 + num12;
            right.z = num8 - num11;

            // Base vertices calculation
            float scaledShadowSize = scaledDoubleShadowSize * 0.5f;
            float aspectRatioInv = 1f / _aspectRatio;

            Vector3 diff;
            diff.x = (forward.x - right.x * aspectRatioInv) * scaledShadowSize;
            diff.y = (forward.y - right.y * aspectRatioInv) * scaledShadowSize;
            diff.z = (forward.z - right.z * aspectRatioInv) * scaledShadowSize;
            Vector3 sum;
            sum.x = (forward.x + right.x * aspectRatioInv) * scaledShadowSize;
            sum.y = (forward.y + right.y * aspectRatioInv) * scaledShadowSize;
            sum.z = (forward.z + right.z * aspectRatioInv) * scaledShadowSize;

            Vector3 baseVertex;
            baseVertex.x = transformPosition.x - sum.x;
            baseVertex.y = transformPosition.y - sum.y;
            baseVertex.z = transformPosition.z - sum.z;
            _baseVertices[0] = baseVertex;

            baseVertex.x = transformPosition.x + diff.x;
            baseVertex.y = transformPosition.y + diff.y;
            baseVertex.z = transformPosition.z + diff.z;
            _baseVertices[1] = baseVertex;

            baseVertex.x = transformPosition.x + sum.x;
            baseVertex.y = transformPosition.y + sum.y;
            baseVertex.z = transformPosition.z + sum.z;
            _baseVertices[2] = baseVertex;

            baseVertex.x = transformPosition.x - diff.x;
            baseVertex.y = transformPosition.y - diff.y;
            baseVertex.z = transformPosition.z - diff.z;
            _baseVertices[3] = baseVertex;

            // Calculate a plane from normal and position
            SS_Plane shadowPlane = new SS_Plane();
            shadowPlane.SetNormalAndPosition(_normal, hitInfo.point + _normal * _shadowOffset);

            float distanceToPlane;
            SS_Ray ray = new SS_Ray();

            // Calculate the shadow vertices
            if (_isPerspectiveProjection && useLightSource) {
                ray.direction = lightSourcePosition - _baseVertices[0];
                ray.origin = _baseVertices[0];
                shadowPlane.Raycast(ref ray, out distanceToPlane);
                _shadowVertices[0] = ray.origin + ray.direction * distanceToPlane;

                ray.direction = lightSourcePosition - _baseVertices[1];
                ray.origin = _baseVertices[1];
                shadowPlane.Raycast(ref ray, out distanceToPlane);
                _shadowVertices[1] = ray.origin + ray.direction * distanceToPlane;

                ray.direction = lightSourcePosition - _baseVertices[2];
                ray.origin = _baseVertices[2];
                shadowPlane.Raycast(ref ray, out distanceToPlane);
                _shadowVertices[2] = ray.origin + ray.direction * distanceToPlane;

                ray.direction = lightSourcePosition - _baseVertices[3];
                ray.origin = _baseVertices[3];
                shadowPlane.Raycast(ref ray, out distanceToPlane);
                _shadowVertices[3] = ray.origin + ray.direction * distanceToPlane;
            } else {
                ray.direction = actualLightVector;

                ray.origin = _baseVertices[0];
                shadowPlane.Raycast(ref ray, out distanceToPlane);
                _shadowVertices[0] = ray.origin + ray.direction * distanceToPlane;

                ray.origin = _baseVertices[1];
                shadowPlane.Raycast(ref ray, out distanceToPlane);
                _shadowVertices[1] = ray.origin + ray.direction * distanceToPlane;

                ray.origin = _baseVertices[2];
                shadowPlane.Raycast(ref ray, out distanceToPlane);
                _shadowVertices[2] = ray.origin + ray.direction * distanceToPlane;

                ray.origin = _baseVertices[3];
                shadowPlane.Raycast(ref ray, out distanceToPlane);
                _shadowVertices[3] = ray.origin + ray.direction * distanceToPlane;
            }

            // Calculate the shadow alpha
            float shadowAlpha = _initialAlpha;

            // Alpha base on distance to the surface
            float distance = hitInfo.distance;
            if (distance > _fadeDistance) {
                shadowAlpha = shadowAlpha - (distance - _fadeDistance) / (_projectionDistance - _fadeDistance) * shadowAlpha;
            }

            // Alpha based on shadow fall angle
            if (angleToNormal > _angleFadeMin) {
                shadowAlpha = shadowAlpha - (angleToNormal - _angleFadeMin) / (_angleFadeMax - _angleFadeMin) * shadowAlpha;
            }

            // Convert float alpha to byte
            _color.a = shadowAlpha;
            _color32.a = (byte) (shadowAlpha * 255f);

            _isVisible = true;
        }

        return RecalculateShadowResult.Recalculated;
    }

    public int GetMeshManagerHashCode() {
        int layer = _useForceLayer ? _forceLayer : gameObject.layer;
        return SS_ShadowMeshManager.GetMeshManagerHashCode(_isStatic, _material, layer);
    }

    /// <summary>
    /// Updates the directional light state.
    /// </summary>
    private void UpdateDirectionalLight() {
        if (_lightSourceObject != null) {
            Light light = _lightSourceObject.GetComponent<Light>();
            _lightSourceObjectIsDirectionalLight = light != null;
            if (light != null && light.type != LightType.Directional) {
                _lightSourceObjectIsDirectionalLight = false;
            }
        } else {
            _lightSourceObjectIsDirectionalLight = false;
        }
    }

    /// <summary>
    /// Calculates initial field values.
    /// </summary>
    private void UpdateProperties() {
        _color32 = _color;
        _initialAlpha = _color.a;
        SetTextureUV(_textureUVRect);
        UpdateDirectionalLight();
    }

    private void OnEnable() {
        _isInitialized = false;

        if (_material == null) {
            _material = Resources.Load("Materials/SS_Shadow Multiply", typeof(Material)) as Material;
            if (_material == null) {
                Debug.LogError("Standard \"Materials/SS_Shadow Multiply\" material was not found. Please assign a shadow material manually for shadow to work.", this);
                return;
            }
        }

        if (_layerMask == 0) {
            Debug.LogError("LayerMask is empty! Shadow won't cast on any surface", this);
            return;
        }

        UpdateComponents();
        UpdateProperties();

        _isGameObjectActivePrev = SS_Extensions.IsActiveInHierarchy(gameObject);
        _isFirstCalculation = true;
        
        RegisterShadow();

        _isInitialized = true;
    }

    private void OnDestroy() {
        UnregisterShadow();
        _isDestroyed = true;
    }

    private void OnDisable() {
        if (!_isInitialized || _isDestroyed ||
#if !UNITY_3_5
            !gameObject.activeSelf
#else
            !gameObject.active
#endif
            ) {
            return;
        }

        UnregisterShadow();
    }

    /// <summary>
    /// Called when the user hits the Reset button in the Inspector's context menu or when adding the component the first time. 
    /// </summary>
    private void Reset() {
        int goLayerMask = 1 << gameObject.layer;
        _forceLayer = goLayerMask;
        _layerMask = unchecked(~0);
    }

    /// <summary>
    /// Updates the cached GameObject components.
    /// </summary>
    private void UpdateComponents() {
        _transform = transform;
    }

    /// <summary>
    /// Calculates UV vectors from Rect.
    /// </summary>
    /// <param name="uvRect">
    /// UV rect.
    /// </param>
    private void SetTextureUV(Rect uvRect) {
        _textureUV[0] = new Vector2(uvRect.x, uvRect.y);
        _textureUV[1] = new Vector2(uvRect.x, uvRect.y + uvRect.height);
        _textureUV[2] = new Vector2(uvRect.x + uvRect.width, uvRect.y + uvRect.height);
        _textureUV[3] = new Vector2(uvRect.x + uvRect.width, uvRect.y);
    }

    /// <summary>
    /// The on draw gizmos selected.
    /// </summary>
    private void OnDrawGizmosSelected() {
        if (!enabled ||
#if !UNITY_3_5
            !gameObject.activeSelf
            #else
            !gameObject.active
#endif
            ) {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _actualLightVectorPrev * _projectionDistance);

        if (_isVisible) {
            Color color = Color.yellow;
            color.a = 0.7f;
            Gizmos.color = color;
            Gizmos.DrawLine(_baseVertices[0], _baseVertices[1]);
            Gizmos.DrawLine(_baseVertices[1], _baseVertices[2]);
            Gizmos.DrawLine(_baseVertices[2], _baseVertices[3]);
            Gizmos.DrawLine(_baseVertices[3], _baseVertices[0]);

            Gizmos.DrawLine(_baseVertices[0], _shadowVertices[0]);
            Gizmos.DrawLine(_baseVertices[1], _shadowVertices[1]);
            Gizmos.DrawLine(_baseVertices[2], _shadowVertices[2]);
            Gizmos.DrawLine(_baseVertices[3], _shadowVertices[3]);

            Gizmos.DrawLine(_shadowVertices[0], _shadowVertices[1]);
            Gizmos.DrawLine(_shadowVertices[1], _shadowVertices[2]);
            Gizmos.DrawLine(_shadowVertices[2], _shadowVertices[3]);
            Gizmos.DrawLine(_shadowVertices[3], _shadowVertices[0]);
        }
    }
}