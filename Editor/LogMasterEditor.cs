using System;
using oculog.Core;
using UnityEditor;
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
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawScriptableObjectFields()
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