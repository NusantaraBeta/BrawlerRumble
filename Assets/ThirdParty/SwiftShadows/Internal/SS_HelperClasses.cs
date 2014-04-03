using System;
using System.Runtime.InteropServices;
using UnityEngine;

#if !UNITY_FLASH && !UNITY_IPHONE && !UNITY_WINRT && !UNITY_WEBPLAYER
using System.Reflection;
#endif

namespace LostPolygon.Internal.SwiftShadows {
    public static class SS_Extensions {
    public static void SetActive(GameObject go, bool active) {
#if UNITY_3_5
        go.active = active;

        foreach (Transform transform in go.transform) {
            go.active = active;
            SetActive(transform.gameObject, active);
        }
#else
        go.SetActive(active);
#endif
    }

        public static bool IsActiveInHierarchy(GameObject gameObject) {
#if !UNITY_3_5
            return gameObject.activeInHierarchy;
#else
            return gameObject.active;
#endif
        }
    }
    public static class SS_Math {
        public const float kDoublePi = Mathf.PI * 2f;
        public const float kInvDoublePi = 1f / kDoublePi;
        public const float kHalfPi = Mathf.PI / 2f;
        public const float kDeg2Rad = 1f / 180f * Mathf.PI;

#if !UNITY_FLASH
        /// <summary>
        /// The union of float and int sharing the same location in memory.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct FloatIntUnion {
            [FieldOffset(0)]
            public float f;

            [FieldOffset(0)]
            public int i;
        }
#endif

        /// <summary>
        /// Calculates the approximate square root of a given value.
        /// </summary>
        /// <remarks>
        /// This function is much faster than <c>Mathf.Sqrt()</c>, especially on mobile devices.
        /// </remarks>
        /// <param name="x">
        /// Input value.
        /// </param>
        /// <returns>
        /// The approximate value of square root of <paramref name="x"/>.
        /// </returns>
        public static float FastSqrt(float x) {
#if !UNITY_FLASH
            FloatIntUnion u;
            u.i = 0;
            u.f = x;
            float xhalf = 0.5f * x;
            u.i = 0x5f375a86 - (u.i >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f * x;
#else
            return Mathf.Sqrt(x);
#endif
        }

        public static float FastInvMagnitude(this Vector3 vector) {
#if !UNITY_FLASH
            float magnitude = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;

            FloatIntUnion u;
            u.i = 0;
            u.f = magnitude;
            float xhalf = 0.5f * magnitude;
            u.i = 0x5f375a86 - (u.i >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f;
#else
            return 1f / vector.magnitude;
#endif
        }

        public static float FastMagnitude(this Vector3 vector) {
#if !UNITY_FLASH
            float magnitude = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;

            FloatIntUnion u;
            u.i = 0;
            u.f = magnitude;
            float xhalf = 0.5f * magnitude;
            u.i = 0x5f375a86 - (u.i >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f * magnitude;
#else
            return vector.magnitude;
#endif
        }

        public static Vector3 FastNormalized(this Vector3 vector) {
#if !UNITY_FLASH
            float magnitude = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;

            FloatIntUnion u;
            u.i = 0;
            u.f = magnitude;
            float xhalf = 0.5f * magnitude;
            u.i = 0x5f375a86 - (u.i >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);

            vector.x = vector.x * u.f;
            vector.y = vector.y * u.f;
            vector.z = vector.z * u.f;

            return vector;
#else
            return vector.normalized;
#endif
        }


        public static float FastAcos(float x) {
            if (x >= 0) {
                x = kHalfPi * FastSqrt(1f - x);
            }
            else {
                x = Mathf.PI - kHalfPi * FastSqrt(1f + x);
            }

            return x;
        }
    }

    /// <summary>
    /// A simple substitute for Ray. Does not normalizes the direction vector.
    /// </summary>
    public struct SS_Ray {
        public Vector3 origin;
        public Vector3 direction;

        public SS_Ray(Vector3 origin, Vector3 direction) {
            this.origin = origin;
            this.direction = direction;
        }
    }

    /// <summary>
    /// A simple substitute for Plane.
    /// </summary>
    public struct SS_Plane {
        private Vector3 _normal;
        private float _distance;

        public void SetNormalAndPosition(Vector3 normal, Vector3 point) {
            _normal = normal;
            _distance = -Vector3.Dot(normal, point);
        }

        public bool Raycast(ref SS_Ray ray, out float enter) {
            float coeff2 = Vector3.Dot(ray.direction, _normal);
            float coeff1 = -Vector3.Dot(ray.origin, _normal) - _distance;
            if (coeff2 < Vector3.kEpsilon && coeff2 > -Vector3.kEpsilon) {
                enter = 0f;
                return false;
            }

            enter = coeff1 / coeff2;
            return enter > 0f;
        }
    }


    /// <summary>
    /// Does nothing. Used for detecting the assembly recompile event.
    /// </summary>
    public class RecompiledMarker {
    }

#if !UNITY_FLASH && !UNITY_IPHONE && !UNITY_WINRT && !UNITY_WEBPLAYER
    /// <summary>
    /// A reflection wrapper around the GeometryUtility internal methods. 
    /// </summary>
    public static class GeometryUtilityInternal {
        private static readonly Action<Plane[], Matrix4x4> Internal_ExtractPlanesDelegate;
        static GeometryUtilityInternal() {
            MethodInfo methodInfo =
                typeof(GeometryUtility)
                    .GetMethod(
                        "Internal_ExtractPlanes",
                        BindingFlags.NonPublic | BindingFlags.Static);

            if (methodInfo != null)
                Internal_ExtractPlanesDelegate = (Action<Plane[], Matrix4x4>) Delegate.CreateDelegate(typeof(Action<Plane[], Matrix4x4>), methodInfo, false);
        }

        /// <summary>
        /// Extract frustum planes from world-to-projection matrix.
        /// </summary>
        /// <param name="planes">
        /// The Plane[] to save planes to.
        /// </param>
        /// <param name="worldToProjectionMatrix">
        /// World to projection matrix.
        /// </param>
        public static void ExtractPlanes(Plane[] planes, Matrix4x4 worldToProjectionMatrix) {
            if (Internal_ExtractPlanesDelegate != null) {
                Internal_ExtractPlanesDelegate(planes, worldToProjectionMatrix);
            } else {
                Plane[] tempPlanes = GeometryUtility.CalculateFrustumPlanes(worldToProjectionMatrix);
                Array.Copy(tempPlanes, planes, 6);
            }
        }

        /// <summary>
        /// Extract frustum planes from camera transform.
        /// </summary>
        /// <param name="planes">
        /// The Plane[] to save planes to.
        /// </param>
        /// <param name="camera">
        /// Camera to calculate frustum from.
        /// </param>
        public static void CalculateFrustumPlanes(Plane[] planes, Camera camera) {
            if (Internal_ExtractPlanesDelegate != null) {
                Internal_ExtractPlanesDelegate(planes, camera.projectionMatrix * camera.worldToCameraMatrix);
            } else {
                Plane[] tempPlanes = GeometryUtility.CalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix);
                Array.Copy(tempPlanes, planes, 6);
            }
        }
    }
#endif
}