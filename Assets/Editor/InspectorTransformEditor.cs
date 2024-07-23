using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable CheckNamespace

namespace Extensions
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform), true)]
    public class InspectorTransformEditor : Editor
    {
        #region Variables

        //
        private SerializedProperty _position;
        private SerializedProperty _rotation;
        private SerializedProperty _scale;
        
        //
        [Flags] private enum Axes { None = 0, X = 1, Y = 2, Z = 4, All = 7 }

        #endregion

        #region Functions

        //
        private void OnEnable ()
        {
            if (this)
            {
                var so = serializedObject;
                _position = so.FindProperty("m_LocalPosition");
                _rotation = so.FindProperty("m_LocalRotation");
                _scale = so.FindProperty("m_LocalScale");
            }
        }
        
        //
        public override void OnInspectorGUI ()
        {
            SetLabelWidth(15f);
            serializedObject.Update();
            DrawPosition();
            DrawRotation();
            DrawScale();
            serializedObject.ApplyModifiedProperties();
        }
        
        //
        private void DrawPosition()
        {
            GUILayout.BeginHorizontal();
            var reset = GUILayout.Button("P", GUILayout.Width(20f));
            EditorGUILayout.PropertyField(_position.FindPropertyRelative("x"));
            EditorGUILayout.PropertyField(_position.FindPropertyRelative("y"));
            EditorGUILayout.PropertyField(_position.FindPropertyRelative("z"));
            GUILayout.EndHorizontal();
            if (reset) _position.vector3Value = Vector3.zero;
        }
        
        //
        private void DrawRotation()
        {
            GUILayout.BeginHorizontal();
            {
                var reset = GUILayout.Button("R", GUILayout.Width(20f));
                var visible = ((Transform)serializedObject.targetObject).localEulerAngles;
		
                visible.x = WrapAngle(visible.x);
                visible.y = WrapAngle(visible.y);
                visible.z = WrapAngle(visible.z);
		
                var changed = CheckDifference(_rotation);
                var altered = Axes.None;
		
                var opt = GUILayout.MinWidth(30f);
		
                if (FloatField("X", ref visible.x, (changed & Axes.X) != 0, false, opt)) altered |= Axes.X;
                if (FloatField("Y", ref visible.y, (changed & Axes.Y) != 0, false, opt)) altered |= Axes.Y;
                if (FloatField("Z", ref visible.z, (changed & Axes.Z) != 0, false, opt)) altered |= Axes.Z;
		
                if (reset)
                {
                    _rotation.quaternionValue = Quaternion.identity;
                }
                else if (altered != Axes.None)
                {
                    RegisterUndo("Change Rotation", serializedObject.targetObjects);
                    foreach (var obj in serializedObject.targetObjects)
                    {
                        var t = obj as Transform;
                        if (t != null)
                        {
                            var v = t.localEulerAngles;
		
                            if ((altered & Axes.X) != 0) v.x = visible.x;
                            if ((altered & Axes.Y) != 0) v.y = visible.y;
                            if ((altered & Axes.Z) != 0) v.z = visible.z;
		
                            t.localEulerAngles = v;
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        
        //
        private void DrawScale()
        {
            GUILayout.BeginHorizontal();
            {
                var reset = GUILayout.Button("S", GUILayout.Width(20f));
                EditorGUILayout.PropertyField(_scale.FindPropertyRelative("x"));
                EditorGUILayout.PropertyField(_scale.FindPropertyRelative("y"));
                EditorGUILayout.PropertyField(_scale.FindPropertyRelative("z"));
                if (reset) _scale.vector3Value = Vector3.one;
            }
            GUILayout.EndHorizontal();
        }
        
        //
        private Axes CheckDifference (SerializedProperty property)
        {
            var axes = Axes.None;
            if (property.hasMultipleDifferentValues)
            {
                var original = property.quaternionValue.eulerAngles;
                foreach (var obj in serializedObject.targetObjects)
                {
                    axes |= CheckDifference(obj as Transform, original);
                    if (axes == Axes.All) break;
                }
            }
            return axes;
        }
        
        //
        private Axes CheckDifference (Transform t, Vector3 original)
        {
            var next = t.localEulerAngles;
            var axes = Axes.None;
            if (Differs(next.x, original.x)) axes |= Axes.X;
            if (Differs(next.y, original.y)) axes |= Axes.Y;
            if (Differs(next.z, original.z)) axes |= Axes.Z;
            return axes;
        }
        
        //
        private bool FloatField (string nameV, ref float value, bool hidden, bool greyedOut, GUILayoutOption opt)
        {
            var newValue = value;
            GUI.changed = false;
            if (!hidden)
            {
                if (greyedOut)
                {
                    GUI.color = new Color(0.7f, 0.7f, 0.7f);
                    newValue = EditorGUILayout.FloatField(nameV, newValue, opt);
                    GUI.color = Color.white;
                }
                else
                {
                    newValue = EditorGUILayout.FloatField(nameV, newValue, opt);
                }
            }
            else if (greyedOut)
            {
                GUI.color = new Color(0.7f, 0.7f, 0.7f);
                float.TryParse(EditorGUILayout.TextField(nameV, "--", opt), out newValue);
                GUI.color = Color.white;
            }
            else
            {
                float.TryParse(EditorGUILayout.TextField(nameV, "--", opt), out newValue);
            }
		
            if (GUI.changed && Differs(newValue, value))
            {
                value = newValue;
                return true;
            }
            return false;
        }

        #endregion

        #region Private functions

        //
        private static void SetLabelWidth(float width)
        {
            EditorGUIUtility.labelWidth = width;
        }
        
        //
        private static void RegisterUndo(string name, params Object[] objects)
        {
            if (objects != null && objects.Length > 0) Undo.RecordObjects(objects, name);
        }
        
        //
        private static float WrapAngle (float angle)
        {
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            return angle;
        }
        
        //
        private static bool Differs (float a, float b)
        {
            return Mathf.Abs(a - b) > 0.0001f;
        }

        #endregion
    }
}