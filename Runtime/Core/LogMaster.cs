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
            settings.OnLogFPS += DataLogger.LogEntry;
        }

        private void OnDisable()
        {
            settings.OnLogFPS -= DataLogger.LogEntry;
        }

        private void Update()
        {
            settings.Tick();
        }
    }
}