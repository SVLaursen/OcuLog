using oculog.Core;
using oculog.LogSettings;
using UnityEditor;
using UnityEngine;

namespace oculog.editor
{
    public partial class OculogEditor
    {
        private void DrawQuickSetupScreen()
        {
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.HelpBox("You can either quickly setup your project using the default settings " +
                                    "or choose your own quick variant." +
                                    "\n\nDefault quick setup does not enable any logging functionalities but generates the necessary " +
                                    "components within your project.", MessageType.Info);
            
            EditorGUILayout.Separator();

            var advancedContent = new GUIContent {text = "Advanced", tooltip = "Currently not available"};

            if (GUILayout.Button("Setup Default Logging"))
                CreateDefaultSetup();

            GUI.enabled = false;
            if (GUILayout.Button(advancedContent))
            {
                Debug.Log("go to advanced quick setup");
            }
            GUI.enabled = true;
            
            EditorGUILayout.EndVertical();
        }

        private void CreateDefaultSetup()
        {
            //Create ScriptableObject of the LogMasterSettings
            var settingsObj = ScriptableObject.CreateInstance<LogMasterSettings>();

            if (!AssetDatabase.IsValidFolder("Assets/Oculog"))
                AssetDatabase.CreateFolder("Assets", "Oculog");

            if (!AssetDatabase.LoadAssetAtPath<LogMasterSettings>("Assets/Oculog/LogMasterSettings.asset"))
            {
                AssetDatabase.CreateAsset(settingsObj, "Assets/Oculog/LogMasterSettings.asset");
                AssetDatabase.SaveAssets();
            }

            //Create an instance of the LogMaster in the current scene
            var logMasterObj = new GameObject("Oculog Log Master");
            var logMaster = logMasterObj.AddComponent<LogMaster>();


            //Assign the LogMasterSettings to the instantiated LogMaster
            logMaster.settings =
                AssetDatabase.LoadAssetAtPath<LogMasterSettings>("Assets/Oculog/LogMasterSettings.asset");
        }
    }
}