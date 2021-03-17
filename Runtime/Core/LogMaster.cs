using System;
using System.Collections.Generic;
using oculog.LogSettings;
using UnityEngine;

namespace oculog.Core
{
    public class LogMaster : MonoBehaviour
    {
        [SerializeField] private LogMasterSettings settings;

        private void OnEnable()
        {
            DataLogger.Init();
            
            settings.Init();
            settings.OnLogFPS += DataLogger.LogEntry;

            if (settings.trackInIntervals)
                StartCoroutine(settings.TrackEveryInterval());
            
#if UNITY_EDITOR
            PlayStateNotifier.ShouldNotExit = true;
            PlayStateNotifier.editorExitEvents += PerformExport;
#elif UNITY_ANDROID
            Application.focusChanged += PerformExportAndroid;
#elif UNITY_STANDALONE
            Application.quitting += PerformExport;
#endif
        }

        private void OnDisable() => settings.OnLogFPS -= DataLogger.LogEntry;

        private void Update() => settings.Tick();

        private void PerformExportAndroid(bool isFocused)
        {
            if(isFocused == false) PerformExport();
        }

        private void PerformExport()
        {
            if (settings.exportType == EExportType.None)
            {
#if UNITY_EDITOR
                PlayStateNotifier.ShouldNotExit = false;
#endif
                return;
            }
            
            var containers = DataLogger.GetDataContainers();
            if(settings.useCustomFolder)
                DataExporter.ExportData(settings.exportType, containers, settings.customFolderName);
            else 
                DataExporter.ExportData(settings.exportType, containers);
        }
    }
}