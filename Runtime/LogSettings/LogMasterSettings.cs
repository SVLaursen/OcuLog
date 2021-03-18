using System;
using System.Collections;
using oculog.Targeting;
using UnityEngine;

namespace oculog.LogSettings
{
    [CreateAssetMenu(menuName = "Oculog/LogMaster Settings")]
    public class LogMasterSettings : ScriptableObject
    {
        //Basic Must Have Settings
        
        
        //Application Tracking Settings
        public bool trackFps;
        public bool trackInIntervals;
        public float trackingInterval = 0.01f;
        public int fpsWarningLimit = 50;
        public int fpsErrorLimit = 40; //If the fps is below this limit then it is failing
        
        //Saving And Exporting Settings
        public EExportType exportType;
        public bool useCustomFolder;
        public string customFolderName;

        public Action<DataEntry> OnLogFPS;

        private float _fpsTime;
        
        public void Tick()
        {
            if(trackFps && !trackInIntervals)
                LogFps();
        }

        public IEnumerator TrackEveryInterval()
        {
            while (Application.isPlaying)
            {
                LogFps();
                yield return new WaitForSeconds(trackingInterval);
            }
        }

        private void LogFps()
        {
            _fpsTime += (Time.unscaledDeltaTime - _fpsTime) * 0.1f;
            var fps = Mathf.FloorToInt(1.0f / _fpsTime);
            var logLevel = ELogLevel.Default;
            
            if (fps < fpsErrorLimit)
                logLevel = ELogLevel.Error;
            else if (fps < fpsWarningLimit)
                logLevel = ELogLevel.Warning;
            
            
            var entry = new DataEntry("fps",fps.ToString(), Time.time, logLevel);
            OnLogFPS.Invoke(entry);
        }
    }
}