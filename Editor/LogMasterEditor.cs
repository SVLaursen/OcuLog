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

            EditorGUILayout.HelpBox("Input tracking is still in development", MessageType.Info);
            GUI.enabled = false;
            
            //Input Fields
            _target.trackButtonInputs = EditorGUILayout.BeginToggleGroup("Button Tracking", _target.trackButtonInputs);
            EditorGUI.indentLevel++;

            LeftAndRightFields("Controller Triggers", ref _target.trackLeftTrigger, ref _target.trackRightTrigger);
            LeftAndRightFields("Controller Grips", ref _target.trackLeftGrip, ref _target.trackRightGrip);
            LeftAndRightFields("Controller Face Buttons", ref _target.trackLeftFaceButtons, ref _target.trackRightFaceButtons);
            LeftAndRightFields("Controller Joysticks", ref _target.trackLeftJoystick, ref _target.trackRightJoystick);
            
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();

            GUI.enabled = true;
            
            //Controller Fields
            _target.trackControllers =
                EditorGUILayout.BeginToggleGroup("Use Controller Tracking", _target.trackControllers);
            EditorGUI.indentLevel++;

            GUI.enabled = false; //TODO: Figure out what goes wrong here before turning it back on
            _target.useIntervalController =
                EditorGUILayout.BeginToggleGroup("Use Interval Tracking", _target.useIntervalController);
            EditorGUI.indentLevel++;
            _target.controllerInterval = EditorGUILayout.FloatField("Interval", _target.controllerInterval);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
            GUI.enabled = true;
            
            LeftAndRightFields("Controller Velocities", ref _target.trackLeftControllerVelocity,
                ref _target.trackRightControllerVelocity);

            LeftAndRightFields("Controller Rotations", ref _target.trackLeftControllerRotation, 
                ref _target.trackRightControllerRotation);
            
            LeftAndRightFields("Controller Positions", ref _target.trackLeftControllerPosition, 
                ref _target.trackRightControllerPosition);

            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
            
            //HMD Fields
            _target.trackHmd = EditorGUILayout.BeginToggleGroup("Track Head Mounted Display", _target.trackHmd);
            EditorGUI.indentLevel++;

            GUI.enabled = false; //TODO: Figure out what goes wrong here before turning it back on
            _target.useIntervalHmd = EditorGUILayout.BeginToggleGroup("Use Interval Tracking", _target.useIntervalHmd);
            EditorGUI.indentLevel++;
            _target.hmdInterval = EditorGUILayout.FloatField("Interval", _target.hmdInterval);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
            GUI.enabled = true;

            _target.trackHmdVelocity = EditorGUILayout.Toggle("Track Velocity", _target.trackHmdVelocity);
            _target.trackHmdPosition = EditorGUILayout.Toggle("Track Position", _target.trackHmdPosition);
            _target.trackHmdRotation = EditorGUILayout.Toggle("Track Rotation", _target.trackHmdRotation);
            
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
            
            EditorGUILayout.EndToggleGroup();
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