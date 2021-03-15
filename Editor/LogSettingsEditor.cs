using System;
using System.Collections.Generic;
using UnityEditor;
using oculog.LogSettings;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace oculog.editor
{
    [CustomEditor(typeof(LogSettings.LogSettings), true)]
    public class LogSettingsEditor : Editor
    {
        private LogSettings.LogSettings _target;
        private List<string> _propsToExclude;

        public void OnEnable()
        {
            _target = (LogSettings.LogSettings) target;
            
            _propsToExclude = new List<string>()
            {
                "logId", "m_Script", "description"
            };
            
            if(_target is DynamicLoggerSettings)
                _propsToExclude.AddRange(new List<string>{"useIntervalLogging", "loggingInterval"});
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel); 
            EditorGUILayout.EndVertical();

            _target.logId = EditorGUILayout.TextField("ID", _target.logId);
            _target.description = EditorGUILayout.TextField("Description", _target.description);

            EditorGUILayout.Separator();

            DrawDynamicLoggerExtras();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Logging Settings", EditorStyles.boldLabel); 
            EditorGUILayout.EndVertical();
            
            DrawPropertiesExcluding(serializedObject, _propsToExclude.ToArray());
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDynamicLoggerExtras()
        {
            if (!(_target is DynamicLoggerSettings dynamicLoggerSettings)) return;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Logging Type", EditorStyles.boldLabel); 
            EditorGUILayout.EndVertical();

            var useInterval = EditorGUILayout.Toggle("Use Interval Logging",
                dynamicLoggerSettings.useIntervalLogging);

            dynamicLoggerSettings.useIntervalLogging = useInterval;

            if (!useInterval)
                GUI.enabled = false;
                
            dynamicLoggerSettings.loggingInterval = EditorGUILayout.FloatField("Logging Interval",
                dynamicLoggerSettings.loggingInterval);

            GUI.enabled = true;
        }
    }
}