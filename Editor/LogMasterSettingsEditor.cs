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
                    DrawAPISettings(oculus, "Oculus");
                    break;
                case ETargetApi.OpenVR:
                    var openvr = serializedObject.FindProperty("openVRSettings").GetEnumerator();
                    DrawAPISettings(openvr, "OpenVR");
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

            _target.trackInIntervals = EditorGUILayout.BeginToggleGroup("Track Every Frame", _target.trackInIntervals);
            _target.trackingInterval = EditorGUILayout.Slider("Tracking Interval",_target.trackingInterval, 0.0f, 1.0f);
            EditorGUILayout.EndToggleGroup();

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

        private void DrawAPISettings(IEnumerator enumerator, string apiName)
        {
            if (apiName == "OpenVR")
            {
                EditorGUILayout.HelpBox("This API has not yet been implemented as the current state of " +
                                        "the project is only a proof of concept.\nThe feature will be implemented in the future!",
                    MessageType.Error);
                return;
            }
            
            EditorGUILayout.HelpBox($"Remember that for the API specific tracking to work, you'll need the " +
                                    $"{apiName} package installed in your project", MessageType.Info);
            
            
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current as SerializedProperty;
                EditorGUILayout.PropertyField(current);
            }
        }
    }
}
