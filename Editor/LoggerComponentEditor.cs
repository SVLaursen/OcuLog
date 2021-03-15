using System;
using oculog.Core;
using oculog.LogSettings;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace oculog.editor
{
    [CustomEditor(typeof(LoggerComponent))]
    public class LoggerComponentEditor : Editor
    {
        private LoggerComponent _target;
        
        private Editor _soEditor;
        private Object _currentSO;

        public void OnEnable()
        {
            _target = (LoggerComponent) target;
            _currentSO = serializedObject.FindProperty("settings").objectReferenceValue;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawWarningMessages();
            DrawSettings();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawWarningMessages()
        {
            if (!(_target.settings is TriggerLoggerSettings)) return;
            var rb = _target.GetComponent<Rigidbody>();
            var collider = _target.GetComponent<Collider>();
                
            if (rb == null || collider == null)
            {
                EditorGUILayout.HelpBox(
                    "The GameObject that this logger is tied to does not contain either a collider " +
                    "or rigidbody component. Remember to add these in order for the trigger logging to work.",
                    MessageType.Warning);
                return;
            }
            
            if(!collider.isTrigger)
                EditorGUILayout.HelpBox("The collider attached need to be set to being a trigger for it " +
                                        "to log data", MessageType.Warning);
        }

        private void DrawSettings()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Settings Setup", EditorStyles.boldLabel);
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
    }
}