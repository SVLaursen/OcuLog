using System;
using System.Collections;
using UnityEngine;

namespace oculog.Core
{
    /*
     * This part of the partial includes all base runtime events and compiler dependent code.
     * Any other logic that pertains to the LogMaster class has been assigned to its own partials.
     */
    public partial class LogMaster
    {
        private void OnEnable()
        {
            DataLogger.Init();
            
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
        
        private void Awake() => CheckAndInitXR();

        private void Update()
        {
            settings.Tick();
            _onTickLog?.Invoke();
        }

        private IEnumerator IntervalLogging(float waitTime, Action onInterval)
        {
            while (Application.isPlaying)
            {
                onInterval.Invoke();
                yield return new WaitForSeconds(waitTime);
            }
        }

#if UNITY_ANDROID        
        private void PerformExportAndroid(bool isFocused)
        {
            if(isFocused == false) PerformExport();
        }
#endif
        
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