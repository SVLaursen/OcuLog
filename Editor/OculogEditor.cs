using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace oculog.editor
{
    public partial class OculogEditor : EditorWindow
    {
        private static bool _openOnStartup;

        private const string PREFERENCES_KEY = "Oculog.Editor.Preferences";
        
        private int _tabIndex;

        [MenuItem("Oculog/Setup", false)]
        public static void Open()
        {
            var window = GetWindow<OculogEditor>("Oculog Editor");
            window.Show();
        }

        public static void OpenOnLoad()
        {
            LoadStorage();
            
            if(_openOnStartup)
                Open();
        }

        public void OnGUI()
        {
            DrawEditorWindow(); 
        }

        private static void LoadStorage()
        {
            if (EditorPrefs.HasKey(PREFERENCES_KEY))
            {
                var storage = new OculogEditorStorage();
                object storageBox = storage;
                var preferencesJson = EditorPrefs.GetString(PREFERENCES_KEY);
                EditorJsonUtility.FromJsonOverwrite(preferencesJson, storageBox);
                storage = (OculogEditorStorage) storageBox;

                _openOnStartup = storage.openOnStart;
            }
            else
            {
                _openOnStartup = true;
            }
        }

        private void SaveStorage()
        {
            var storage = new OculogEditorStorage {openOnStart = _openOnStartup};
            var preferencesJSON = EditorJsonUtility.ToJson(storage);
            EditorPrefs.SetString(PREFERENCES_KEY, preferencesJSON);
        }

        private void OnDisable()
        {
            SaveStorage();
        } 

        private void DrawEditorWindow()
        {
           //Toolbar
           EditorGUILayout.BeginVertical("box");
           _tabIndex = GUILayout.Toolbar(_tabIndex, new[] {"Home", "Quick Setup"});
           EditorGUILayout.EndVertical();
           
           //Content based on selected tab
           DrawMainContent();
        }

        private void DrawMainContent()
        {
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.BeginHorizontal();

            switch (_tabIndex)
            {
                case 0:
                    DrawStartScreen();
                    break;
                case 1:
                    DrawQuickSetupScreen();
                    break;
                default:
                    break;
            }
            EditorGUILayout.EndHorizontal();
            
            GUILayout.FlexibleSpace();
            
            
            EditorGUILayout.EndVertical();
        }

        private void DrawStartScreen()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Oculog", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("v1.0.0");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.HelpBox("Thank you for using OcuLog.\n\nThis plugin comes with native support for " +
                                    "the XR Toolkit, and allows for tracking of multiple variables within your XR project.\n\n" +
                                    "In order to make sure you get off to a great start we've implemented this quick setup " +
                                    "editor that will quickly make your current scene ready for logging data.\n\n" +
                                    "If you have any questions please refer to the documentation linked below.", MessageType.None);
            
            EditorGUILayout.HelpBox("Be aware that Oculog is still in active development and you may therefore " +
                                    "still experience some issues while using this plugin. \nIf you find a bug, please " +
                                    "report it to slaur15@student.aau.dk", MessageType.Warning);
            
            if(GUILayout.Button("Documentation"))
                Application.OpenURL("https://github.com/VRMON/UnityVRLogger/blob/main/README.md");

            if (GUILayout.Button("Quick Setup"))
                _tabIndex = 1;

            _openOnStartup = EditorGUILayout.Toggle("Display on startup", _openOnStartup);
            EditorGUILayout.EndVertical();
        }
    }
    
    [Serializable]
    struct OculogEditorStorage
    {
        public bool openOnStart;
    }
}