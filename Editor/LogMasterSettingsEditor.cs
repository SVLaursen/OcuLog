using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using oculog;
using oculog.LogSettings;
using oculog.Targeting;

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
            
            //Draw the Platform Selection Area
            DrawPlatformSpecificFields();
            
            //Draw the Application Tracking Area
            DrawApplicationTrackingFields();
            
            //Draw the Saving/Exporting Area
            DrawExportingFields();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPlatformSpecificFields()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Platform Settings", EditorStyles.boldLabel); 
            EditorGUILayout.EndVertical();

            _target.platform = (ETargetApi) EditorGUILayout.EnumPopup("Target Platform", _target.platform);
            
            EditorGUILayout.Separator();
            
            switch (_target.platform)
            {
                case ETargetApi.Oculus:
                    var oculus = serializedObject.FindProperty("oculusSettings").GetEnumerator();
                    DrawAPISettings(oculus);
                    break;
                case ETargetApi.OpenVR:
                    var openvr = serializedObject.FindProperty("openVRSettings").GetEnumerator();
                    DrawAPISettings(openvr);
                    break;
                case ETargetApi.None:
                    EditorGUILayout.HelpBox("No API has been targeted and Oculog will therefore only " +
                                            "track using the default logging components", MessageType.Info);
                    break;
            }
        }

        private void DrawApplicationTrackingFields()
        {
            EditorGUILayout.Separator();
            
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Framerate Tracking Settings", EditorStyles.boldLabel); 
            EditorGUILayout.EndVertical();

            _target.trackFps = EditorGUILayout.BeginToggleGroup("Toggle FPS Tracking", _target.trackFps);

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
        }

        private void DrawAPISettings(IEnumerator enumerator)
        {
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                    
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current as SerializedProperty;
                EditorGUILayout.PropertyField(current);
            }
        }
    }
}
