using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using oculog;
using oculog.LogSettings;
using oculog.Targeting;
using Object = UnityEngine.Object;

namespace oculog.editor
{
    [CustomEditor(typeof(LogMasterSettings))]
    public class LogMasterSettingsEditor : Editor
    {
        private LogMasterSettings _target;
        
        public void OnEnable()
        {
            _target = (LogMasterSettings) target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            //Draw the Application Tracking Area
            DrawApplicationTrackingFields();
            
            //Draw the Saving/Exporting Area
            DrawExportingFields();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawApplicationTrackingFields()
        {
            EditorGUILayout.Separator();
            
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Framerate Tracking Settings", EditorStyles.boldLabel); 
            EditorGUILayout.EndVertical();

            _target.trackFps = EditorGUILayout.BeginToggleGroup("Toggle FPS Tracking", _target.trackFps);
            
            GUI.enabled = false; //TODO: Figure out what goes wrong here before turning it back on
            EditorGUI.indentLevel++;
            _target.trackInIntervals = EditorGUILayout.BeginToggleGroup("Track In Intervals", _target.trackInIntervals);
            EditorGUI.indentLevel++;
            _target.trackingInterval = EditorGUILayout.Slider("Tracking Interval",_target.trackingInterval, 0.0f, 1.0f);

            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
            EditorGUI.indentLevel--;
            GUI.enabled = true;
            
            EditorGUILayout.Separator();
            
            _target.fpsWarningLimit = EditorGUILayout.IntField("Warning Limit", _target.fpsWarningLimit);
            _target.fpsErrorLimit = EditorGUILayout.IntField("Error Limit", _target.fpsErrorLimit);
            
            EditorGUILayout.EndToggleGroup();
        }

        private void DrawExportingFields()
        {
            EditorGUILayout.Separator();
            
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Export Settings", EditorStyles.boldLabel); 
            EditorGUILayout.EndVertical();

            _target.exportType = (EExportType) EditorGUILayout.EnumPopup("Export Type", _target.exportType);

            EditorGUILayout.Separator();

            if (_target.exportType == EExportType.None)
            {
                EditorGUILayout.HelpBox("No export type has been chosen and the recorded data will " +
                                        "therefore not be saved during runtime", MessageType.Info);
                return;
            }
            _target.useCustomFolder = EditorGUILayout.BeginToggleGroup("Use Custom Folder", _target.useCustomFolder);
            _target.customFolderName = EditorGUILayout.TextField("Folder Name", _target.customFolderName);
            EditorGUILayout.EndToggleGroup();
            
            EditorGUILayout.Separator();
            var folderName = _target.useCustomFolder ? _target.customFolderName : "oculog";
            var savePath = $"{Application.persistentDataPath}/{folderName}";
            EditorGUILayout.HelpBox($"Your data will be exported to:\n" +
                                    $"{savePath}", MessageType.None);
        }
    }
}
