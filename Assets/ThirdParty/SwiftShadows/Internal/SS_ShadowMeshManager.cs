using System;
using UnityEngine;

#if !UNITY_3_5
using LostPolygon.SwiftShadows;
#endif

namespace LostPolygon.Internal.SwiftShadows {
    /// <summary>
    /// Manages the mesh generated for the collection of similar shadows.
    /// </summary>
    public class SS_ShadowMeshManager {
        private static readonly Matrix4x4 kMatrixIdentity = Matrix4x4.identity;
        private readonly Material _material;
        private readonly int _layer;
        private readonly bool _isStatic;
# if !UNITY_FLASH
        private readonly SS_NanoList<SwiftShadow> _shadowsList = new SS_NanoList<SwiftShadow>();
# else
        private readonly List<SwiftShadow> _shadowsList = new List<SwiftShadow>();
# endif
        private Mesh _mesh;
        private ShadowMeshStruct _meshStruct;
        private int _visibleShadowsCount;
        private bool _isStaticDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SS_ShadowMeshManager"/> class.
        /// </summary>
        /// <param name="material">
        /// Material to use for shadows.
        /// </param>
        /// <param name="layer">
        /// Layer on which the shadows are rendered.
        /// </param>
        /// <param name="isStatic">
        /// Whether the mesh manager is not updated automatically.
        /// </param>
        public SS_ShadowMeshManager(Material material, int layer, bool isStatic) {
            _material = material;
            _isStatic = isStatic;
            _layer = layer;

            CreateMesh();

            _isStaticDirty = true;
        }

        public int ShadowsCount {
            get {
                return _shadowsList.Count;
            }
        }

        public int VisibleShadowsCount {
            get {
                return _visibleShadowsCount;
            }
        }

        public bool IsStatic {
            get {
                return _isStatic;
            }
        }

        public Material Material {
            get {
                return _material;
            }
        }

        public int Layer {
            get {
                return _layer;
            }
        }

        public Mesh Mesh {
            get {
                return _mesh;
            }
        }

        /// <summary>
        /// Frees the mesh resources.
        /// </summary>
        public void FreeMesh() {
            if (_mesh != null) {
                _mesh.Clear();
                UnityEngine.Object.DestroyImmediate(_mesh);
                _mesh = null;
            }
        }

        /// <summary>
        /// Returns the numeric hash code of mesh manager.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/> representing the unique combination of material, layer, and static state.
        /// </returns>
        public int GetInstanceHashCode() {
            return GetMeshManagerHashCode(_isStatic, _material, _layer);
        }

        /// <summary>
        /// Registers the shadow in this manager.
        /// </summary>
        /// <param name="shadow">
        /// The shadow to register.
        /// </param>
        public void RegisterShadow(SwiftShadow shadow) {
            if (_isStatic)
                shadow.RecalculateShadow(null, true);

            _shadowsList.Add(shadow);
            _isStaticDirty = true;
        }

        /// <summary>
        /// Unregisters the shadow from this manager.
        /// </summary>
        /// <param name="shadow">
        /// The shadow to unregister.
        /// </param>
        public void UnregisterShadow(SwiftShadow shadow) {
            _shadowsList.Remove(shadow);
            _isStaticDirty = true;
        }

        /// <summary>
        /// Recalculates the geometry of every attached shadow and builds a batched mesh
        /// </summary>
        /// <param name="frustumPlanes">
        /// The frustum planes of camera that renders the scene.
        /// </param>
        public void RecalculateGeometry(Plane[] frustumPlanes) {
            RecalculateGeometry(frustumPlanes, false);
        }

        /// <summary>
        /// Forces recalculation of static mesh shadows
        /// </summary>
        public void ForceStaticRecalculate() {
            _isStaticDirty = true;
        }

        /// <summary>
        /// Submits rendering the the batched shadows mesh.
        /// </summary>
        public void DrawMesh(Camera camera) {
            if (_shadowsList.Count == 0 || _visibleShadowsCount == 0)
                return;

            Graphics.DrawMesh(_mesh, kMatrixIdentity, _material, _layer, camera, 0, null, false, false);
        }

        public static int GetMeshManagerHashCode(bool isStatic, Material material, int layer) {
            unchecked {
                const int initialHash = 17; // Prime number
                const int multiplier = 29; // Different prime number

                int hash = initialHash;
                hash = hash * multiplier + (!isStatic ? 0 : 1);
                hash = hash * multiplier + material.GetInstanceID();
                hash = hash * multiplier + layer;

                return hash;
            }
        }

        private void CreateMesh() {
            if (_mesh != null)
                return;

            _mesh = new Mesh();
            _mesh.name = "_ShadowMesh_" + _isStatic.ToString() + "_" + _material.name;
#if !UNITY_3_5 && !UNITY_3_4
        _mesh.MarkDynamic();
#endif
        }

        /// <summary>
        /// Rebuilds the batched mesh.
        /// </summary>
        private void RebuildMesh() {
#if UNITY_FLASH
        if (_mesh == null)
            CreateMesh();
#endif
            if (_mesh == null)
                return;

            _meshStruct.EnsureCapacity(_visibleShadowsCount);

            int index = 0;
            int triangleIndex = 0;
            int shadowsCount = _shadowsList.Count;

            for (int i = 0; i < shadowsCount; i++) {
#         if !UNITY_FLASH
                SwiftShadow shadow = _shadowsList.Items[i];
#         else
                SwiftShadow shadow = _shadowsList[i];
#         endif
                if (!shadow.IsVisible)
                    continue;

                Vector3[] shadowVertices = shadow.ShadowVertices;
                Vector3 normal = shadow.Normal;
                Vector2[] textureUV = shadow.TextureUV;
                Color32 color32 = shadow.Color32;

                _meshStruct.Vertices[index] = shadowVertices[0];
                _meshStruct.Vertices[index + 1] = shadowVertices[1];
                _meshStruct.Vertices[index + 2] = shadowVertices[2];
                _meshStruct.Vertices[index + 3] = shadowVertices[3];

                _meshStruct.UV[index] = textureUV[0];
                _meshStruct.UV[index + 1] = textureUV[1];
                _meshStruct.UV[index + 2] = textureUV[2];
                _meshStruct.UV[index + 3] = textureUV[3];

                _meshStruct.Normals[index] = normal;
                _meshStruct.Normals[index + 1] = normal;
                _meshStruct.Normals[index + 2] = normal;
                _meshStruct.Normals[index + 3] = normal;

                _meshStruct.Colors32[index] = color32;
                _meshStruct.Colors32[index + 1] = color32;
                _meshStruct.Colors32[index + 2] = color32;
                _meshStruct.Colors32[index + 3] = color32;

                _meshStruct.Indices[triangleIndex] = index + 0;
                _meshStruct.Indices[triangleIndex + 1] = index + 1;
                _meshStruct.Indices[triangleIndex + 2] = index + 2;
                _meshStruct.Indices[triangleIndex + 3] = index + 0;
                _meshStruct.Indices[triangleIndex + 4] = index + 2;
                _meshStruct.Indices[triangleIndex + 5] = index + 3;

                index += 4;
                triangleIndex += 6;
            }

            _mesh.vertices = _meshStruct.Vertices;
            _mesh.normals = _meshStruct.Normals;
            _mesh.uv = _meshStruct.UV;
            _mesh.colors32 = _meshStruct.Colors32;
            _mesh.triangles = _meshStruct.Indices;
            _mesh.RecalculateBounds();
        }

        /// <summary>
        /// Recalculates the geometry of every attached shadow and builds a batched mesh.
        /// </summary>
        /// <param name="frustumPlanes">
        /// The frustum planes of camera that renders the scene.
        /// </param>
        /// <param name="skipRecalculate">
        /// Whether to skip recalculation of shadow geometry.
        /// </param>
        private void RecalculateGeometry(Plane[] frustumPlanes, bool skipRecalculate) {
            // No need to rebuild static mesh when it hasn't changed
            bool mustRebuildMesh = !IsStatic || _isStaticDirty;

            int visibleShadowsCount = 0;
            if (_isStatic)
                frustumPlanes = null;
            int shadowCount = _shadowsList.Count;

            for (int i = 0; i < shadowCount; i++) {
#         if !UNITY_FLASH
                SwiftShadow shadow = _shadowsList.Items[i];
#         else
                SwiftShadow shadow = _shadowsList[i];
#         endif
                if (!skipRecalculate) {
                    SwiftShadow.RecalculateShadowResult recalculateResult = shadow.RecalculateShadow(frustumPlanes, false);
                    switch (recalculateResult) {
                        case SwiftShadow.RecalculateShadowResult.ChangedManager:
                            i--;
                            shadowCount = _shadowsList.Count;
                            continue;
                        case SwiftShadow.RecalculateShadowResult.Recalculated:
                            mustRebuildMesh = true;
                            break;
                    }
                }

                if (shadow.IsVisible)
                    visibleShadowsCount++;
            }
            if (!mustRebuildMesh || (_visibleShadowsCount == 0 && visibleShadowsCount == 0))
                return;

            _visibleShadowsCount = visibleShadowsCount;

            RebuildMesh();
            _isStaticDirty = false;
        }

        private struct ShadowMeshStruct {
            public int[] Indices;
            public Vector3[] Vertices;
            public Vector3[] Normals;
            public Vector2[] UV;
            public Color32[] Colors32;

            private const int kCapacityGrowStep = 16;
            private int _capacity;

            public void EnsureCapacity(int shadowCount) {
                int numIndices = shadowCount * 6;

                // Update indices only if we really need it
                bool mustClearIndices = Indices != null && numIndices < Indices.Length;
                if (shadowCount > _capacity) {
                    // We have to increase the arrays capacity
                    _capacity = (shadowCount / kCapacityGrowStep + 1) * kCapacityGrowStep;

                    // Two triangles per quad
                    numIndices = _capacity * 6;
                    Indices = new int[numIndices];

                    int numVertices = _capacity * 4;
                    Vertices = new Vector3[numVertices];
                    Normals = new Vector3[numVertices];
                    UV = new Vector2[numVertices];
                    Colors32 = new Color32[numVertices];
                }

                if (mustClearIndices) {
#if !UNITY_FLASH
                    Array.Clear(Indices, 0, Indices.Length);
#else
                for (int i = 0; i < Indices.Length; i++) {
                    Indices[i] = 0;
                }
#endif
                }
            }
        }
    }
}