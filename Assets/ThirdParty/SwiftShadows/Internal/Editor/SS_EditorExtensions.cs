#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
#  define PRE_UNITY_4_3
#endif

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LostPolygon.Internal.SwiftShadows.EditorExtensions {
    public static class EditorClassesExtensions {
        public static string[] GetLayerMaskNames(this SerializedProperty property) {
            return (string[]) typeof(SerializedProperty).GetMethod("GetLayerMaskNames", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(property, null);
        }    
    }

    public static class EditorGUILayoutExtensions {
        public const float kIndentationWidth = 9f;
        public const float kLeftPaddingWidth = 4f;

        //FixedWidthLabel class. Extends IDisposable, so that it can be used with the "using" keyword.
        private class FixedWidthLabel : IDisposable {
            private readonly ZeroIndent _indentReset; // Helper class to reset and restore indentation

            public FixedWidthLabel(GUIContent label) {
#             if PRE_UNITY_4_3
                float indentation = kIndentationWidth * EditorGUI.indentLevel + kLeftPaddingWidth;
#             else
                float indentation = kIndentationWidth * EditorGUI.indentLevel;
#             endif

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(indentation);
                float width = Mathf.Max(EditorGUIUtilityInternal.labelWidth - indentation,
                                        GUI.skin.label.CalcSize(label).x);
                GUILayout.Label(label, GUILayout.Width(width));

                _indentReset = new ZeroIndent();
            }

            public FixedWidthLabel(string label) : this(new GUIContent(label)) {
            }

            public void Dispose() {
                _indentReset.Dispose();
                EditorGUILayout.EndHorizontal();
            }
        }

        private class ZeroIndent : IDisposable
        {
            private readonly int _originalIndent;

            public ZeroIndent() {
                _originalIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
            }

            public void Dispose() {
                EditorGUI.indentLevel = _originalIndent;
            }
        }

        public static bool ToggleFixedWidth(Rect rect, GUIContent label, bool value) {
            using (new FixedWidthLabel(label)) {
                value =
                    GUI.Toggle(
                        rect,
                        value,
                        ""
                        );

                return value;
            }
        }

        public static bool ToggleFixedWidth(GUIContent label, bool value) {
            return ToggleFixedWidth(
                EditorGUILayoutInternal.GetControlRect(
                false,
                16f,
                EditorStyles.toggle,
                null
                ),
                label, 
                value);
        }

        public static bool ToggleFixedWidth(string label, bool value) {
            return ToggleFixedWidth(new GUIContent(label), value);
        }

        public static float FloatFieldFixedWidth(Rect rect, GUIContent label, float value) {
            using (new FixedWidthLabel(label)) {
                value =
                    EditorGUI.FloatField(
                        rect,
                        "",
                        value
                        );

                return value;
            }
        }

        public static float FloatFieldFixedWidth(GUIContent label, float value) {
            return FloatFieldFixedWidth(
                EditorGUILayoutInternal.GetControlRect(
                false,
                16f,
                EditorStyles.textField,
                null
                ),
                label, 
                value);
        }

        public static int IntSliderFixedWidth(GUIContent label, int value, int leftValue, int rightValue) {
            using (new FixedWidthLabel(label)) {
                value =
                    EditorGUI.IntSlider(
                        EditorGUILayoutInternal.GetControlRect(
                            false,
                            16f,
                            EditorStyles.toggle,
                            null
                            ),
                        "",
                        value,
                        leftValue,
                        rightValue
                        );

                return value;
            }
        }

        public static int IntSliderFixedWidth(string label, int value, int leftValue, int rightValue) {
            return IntSliderFixedWidth(new GUIContent(label), value, leftValue, rightValue);
        }
    }

    public static class EditorGUIInternal {
        private static readonly Type _type;
        public const float kNumberW = 40f;

        static EditorGUIInternal() {
            _type = typeof(Editor).Assembly.GetType("UnityEditor.EditorGUI", true);
        }

        public static float GetPropertyHeight(SerializedProperty property, GUIContent label, bool includeChildren) {
		    return (float) _type
                .GetMethod(
                    "GetPropertyHeight", 
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public,
                    null,
                    new Type[] {typeof(SerializedProperty), typeof(GUIContent), typeof(bool)},
                    null
                    )
                .Invoke(null, new object[] {property, label, includeChildren});
        }

        public static void ObjectReferenceField(Rect position, SerializedProperty property, GUIContent label, GUIStyle style) {
		    _type
                .GetMethod(
                    "ObjectReferenceField", 
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public,
                    null,
                    new Type[] {typeof(Rect), typeof(SerializedProperty), typeof(GUIContent), typeof(GUIStyle)},
                    null
                    )
                .Invoke(null, new object[] {position, property, label, style});
        }

        public static void LayerMaskField(Rect position, SerializedProperty property, GUIContent label) {
		    _type
                .GetMethod(
                    "LayerMaskField", 
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public,
                    null,
                    new Type[] {typeof(Rect), typeof(SerializedProperty), typeof(GUIContent)},
                    null
                    )
                .Invoke(null, new object[] {position, property, label});
        }
    }

    #region EditorGUIUtility internal
    public static class EditorGUIUtilityInternal {
        public static float labelWidth {
            get {
#             if PRE_UNITY_4_3
                Type type = typeof (EditorGUIUtility);
                FieldInfo info = type.GetField("labelWidth", BindingFlags.Static | BindingFlags.NonPublic);
                if (info != null) {
                    object value = info.GetValue(null);
                    return (float) value;
                }

                return 0f;
#             else
                return EditorGUIUtility.labelWidth;
#             endif
            }
        }

        public static GUIContent[] TempContent(string[] texts) {
        	GUIContent[] array = new GUIContent[texts.Length];
        	for (int i = 0; i < texts.Length; i++) {
        		array[i] = new GUIContent(texts[i]);
        	}
        	return array;
        }

    }
    #endregion

    #region EditorStyles internal
    public static class EditorStylesInternal
    {
        public static GUIStyle helpBox
        {
            get
            {
                Type type = typeof(EditorStyles);
                PropertyInfo info = type.GetProperty("helpBox", BindingFlags.Static | BindingFlags.NonPublic);
                if (info != null)
                {
                    object value = info.GetValue(type, null);
                    return (GUIStyle)value;
                }

                return EditorStyles.label;
            }
        }
    }
    #endregion

    public static class EditorGUILayoutInternal {
        public const float kLabelFloatMinW = 80f + EditorGUIInternal.kNumberW + 5f;
        public const float kLabelFloatMaxW = 80f + EditorGUIInternal.kNumberW + 5f;
        public const float kPlatformTabWidth = 30f;

        public static Rect GetControlRect(bool hasLabel, float height, GUIStyle style, params GUILayoutOption[] options) {
            Rect rect = GUILayoutUtility.GetRect(!hasLabel ? EditorGUIInternal.kNumberW : kLabelFloatMinW,
                                                 kLabelFloatMaxW, height, height, style, options);
            
#         if PRE_UNITY_4_3
            rect.yMin -= 2f;
#         endif

            return rect;
        }
    }
    

    #region PropertyEditor
    public abstract class PropertyEditor<TObject> : Editor where TObject : MonoBehaviour {
        private static Type _serializedPropertyType;
        private Type _type;
        protected List<TObject> _objectList = new List<TObject>();

        static PropertyEditor() {
            _serializedPropertyType = typeof(SerializedProperty);
        }

        protected virtual void OnEnable() {
            _type = typeof(TObject);
            _objectList.Clear();
        }

        protected void OnDestroy() {
            _objectList.Clear();
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            _objectList.Clear();

            if (serializedObject.isEditingMultipleObjects) {
                foreach (Object targetObject in serializedObject.targetObjects) {
                    TObject obj = targetObject as TObject;
                    if (obj != null)
                        _objectList.Add(obj);
                }
            } else {
                TObject obj = serializedObject.targetObject as TObject;
                if (obj != null)
                    _objectList.Add(obj);
            }
        }

        protected Rect BeginProperty(SerializedProperty property, GUIContent label, float height) {
            Rect position = 
                EditorGUILayoutInternal.GetControlRect(
                true, 
                height,
                EditorStyles.layerMaskField, null);
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            return position;
        }

        protected Rect BeginProperty(SerializedProperty property, GUIContent label) {
            return BeginProperty(property, label, EditorGUIInternal.GetPropertyHeight(property, label, false));
        }

        protected bool EndProperty() {
            bool result = EditorGUI.EndChangeCheck();
            EditorGUI.EndProperty();
            return result;
        } 

        protected T DoFieldProperty<T>(SerializedProperty property, GUIContent label, string propertyName, Func<Rect, GUIContent, SerializedProperty, T> guiFunc) {
            if (property == null)
                throw new ArgumentNullException("property", "Property name: " + propertyName);
            if (guiFunc == null)
                throw new ArgumentNullException("guiFunc");

            Rect fieldRect = BeginProperty(property, label);

            object returnValue = null;
            object tempValue = guiFunc(fieldRect, label, property);
            if (property.propertyType == SerializedPropertyType.LayerMask)
                return default(T);

            PropertyInfo serializedPropertyInfo = _serializedPropertyType.GetProperty(GetSerializedPropertyValuePropertyName(property));
            if (EndProperty()) {
                PropertyInfo propertyInfo = _type.GetProperty(propertyName);
                foreach (Object targetObject in serializedObject.targetObjects) {
                    propertyInfo.SetValue(targetObject, tempValue, null);
                    returnValue = ConvertSerializedPropertyValue(property, propertyInfo.GetValue(targetObject, null));
                    serializedPropertyInfo.SetValue(property, returnValue, null);
                    returnValue = serializedPropertyInfo.GetValue(property, null);
                }
            }

            if (returnValue == null)
                return (T) ConvertOutValue<T>(property, serializedPropertyInfo.GetValue(property, null));

            return (T) ConvertOutValue<T>(property, returnValue);
        }

        protected object ConvertOutValue<T>(SerializedProperty property, object obj) {
            if (property.propertyType == SerializedPropertyType.Color) {
                if (typeof(T) == typeof(Color32)) {
                    if (obj is Color) {
                        Color32 color = (Color) obj;
                        return color;
                    }

                    return (Color32) obj;
                }

                return (Color) obj;
            }

            return obj;
        }

        protected object ConvertSerializedPropertyValue(SerializedProperty property, object obj) {
            if (property.propertyType == SerializedPropertyType.Enum) {
                return (int) obj;
            }

            if (property.propertyType == SerializedPropertyType.Color) {
                if (obj is Color32) {
                    Color color = (Color32) obj;
                    return color;
                }

                return (Color) obj;
            }

            return obj;
        }

        protected string GetSerializedPropertyValuePropertyName(SerializedProperty property) {
            switch (property.propertyType) {
                case SerializedPropertyType.Boolean:
                    return "boolValue";
                case SerializedPropertyType.Integer:
                    return "intValue";
                case SerializedPropertyType.Float:
                    return "floatValue";
                case SerializedPropertyType.String:
                    return "stringValue";
                case SerializedPropertyType.Rect:
                    return "rectValue";
                case SerializedPropertyType.Color:
                    return "colorValue";
                case SerializedPropertyType.Vector2:
                    return "vector2Value";
                case SerializedPropertyType.Vector3:
                    return "vector3Value";
                case SerializedPropertyType.Enum:
                    return "enumValueIndex";
                case SerializedPropertyType.ObjectReference:
                    return "objectReferenceValue";
                default: throw new ArgumentException("Unknown property type " + property.propertyType);
            }
        }
    }
    #endregion
}