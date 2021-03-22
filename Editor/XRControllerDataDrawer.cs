using oculog.Core;
using UnityEditor;
using UnityEngine;

namespace oculog.editor
{
    [CustomPropertyDrawer(typeof(XRControllerData))]
    public class XRControllerDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label, EditorStyles.boldLabel);
            
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.LabelField("Input", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            EditorGUILayout.PropertyField(property.FindPropertyRelative("logButtonInputs"));
            var enableInputFields = property.FindPropertyRelative("logButtonInputs").boolValue;
            if (!enableInputFields) GUI.enabled = false;

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(property.FindPropertyRelative("trigger"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("grip"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("joystick"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("faceButtons"));
            EditorGUI.indentLevel--;
            GUI.enabled = true;
            EditorGUI.indentLevel--;
            
            
            EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(property.FindPropertyRelative("logMovement"));
            var enableMovementFields = property.FindPropertyRelative("logMovement").boolValue;
            if (!enableMovementFields) GUI.enabled = false;
            
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(property.FindPropertyRelative("position"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("rotation"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("velocity"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("acceleration"));
            
            EditorGUI.indentLevel--;
            GUI.enabled = true;
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUI.EndProperty();
        }
    }
}