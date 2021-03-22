using System;
using oculog.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace oculog.editor
{
    [CustomEditor(typeof(LogMaster))]
    public class LogMasterEditor : Editor
    {
        private LogMaster _target;
        
        private Editor _soEditor;
        private Object _currentSO;

        public void OnEnable()
        {
            _target = (LogMaster) target;
            _currentSO = serializedObject.FindProperty("settings").objectReferenceValue;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptableObjectFields();

            DrawXRFields();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawScriptableObjectFields()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Required Settings", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();

            EditorGUILayout.ObjectField(serializedObject.FindProperty("settings"));

            var settingsProperty = serializedObject.FindProperty("settings");
            if (settingsProperty.objectReferenceValue == null) return;

            if (!_soEditor || settingsProperty.objectReferenceValue != _currentSO)
            {
                CreateCachedEditor(settingsProperty.objectReferenceValue, null, ref _soEditor);
                _currentSO = settingsProperty.objectReferenceValue;
            }
            
            _soEditor.OnInspectorGUI();
        }

        private void DrawXRFields()
        {
            EditorGUILayout.Separator();
            
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Unity XR Support", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();

            _target.useXR = EditorGUILayout.BeginToggleGroup("Use Unity XR", _target.useXR);
            
            //Guardian Fields
            _target.enableGuardian = EditorGUILayout.BeginToggleGroup("Guardian Tracking", _target.enableGuardian);

            EditorGUI.indentLevel++;
            _target.guardianIterations =
                EditorGUILayout.IntSlider("Guardian Accuracy", _target.guardianIterations,
                    1, 20);


            _target.guardianHeight = EditorGUILayout.FloatField("Guardian Height", _target.guardianHeight);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
            
            //HMD Fields
            _target.trackHmd = EditorGUILayout.BeginToggleGroup("Track Head Mounted Display", _target.trackHmd);
            EditorGUI.indentLevel++;

            _target.trackHmdVelocity = EditorGUILayout.Toggle("Track Velocity", _target.trackHmdVelocity);
            _target.trackHmdPosition = EditorGUILayout.Toggle("Track Position", _target.trackHmdPosition);
            _target.trackHmdRotation = EditorGUILayout.Toggle("Track Rotation", _target.trackHmdRotation);
            
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
            
            EditorGUILayout.EndToggleGroup();

            //Controller Fields
            var label = new GUIContent("Left Controller");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("leftController"), label);
            
            label.text = "Right Controller";
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rightController"), label);
        }

        private void LeftAndRightFields(string label, ref bool leftField, ref bool rightField)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            //EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("L", GUILayout.Width(25));
            leftField = EditorGUILayout.Toggle(leftField, GUILayout.Width(25));
            
            GUILayout.Space(25);
            
            EditorGUILayout.LabelField("R", GUILayout.Width(25));
            rightField = EditorGUILayout.Toggle(rightField, GUILayout.Width(25));
            
            EditorGUILayout.EndHorizontal();
            //EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}