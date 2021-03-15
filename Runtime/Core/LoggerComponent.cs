using System;
using oculog.LogSettings;
using UnityEngine;

namespace oculog.Core
{
    public class LoggerComponent : MonoBehaviour
    {
        public LogSettings.LogSettings settings;

        private TriggerLoggerSettings _triggerLoggerRef;
        private bool _useTick = true;
        
        private void Start()
        {
            settings.Init(gameObject);
            settings.OnEmit += DataLogger.LogEntry;

            if (settings is TriggerLoggerSettings loggerSettings)
                _triggerLoggerRef = loggerSettings;

            if (!(settings is DynamicLoggerSettings dynamicLogger)) return;
            if (dynamicLogger.useIntervalLogging)
                StartCoroutine(dynamicLogger.LogInIntervals());
            _useTick = !dynamicLogger.useIntervalLogging;
        }

        private void OnDisable()
        {
            settings.OnEmit -= DataLogger.LogEntry;
        }

        private void Update()
        {
            if (!_useTick) return;
            settings.Tick();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_triggerLoggerRef.triggerType != TriggerType.OnEnter) return;
            _triggerLoggerRef.OnCollisionEvent($"Collided with {other.gameObject.name}");
        }

        private void OnTriggerStay(Collider other)
        {
            if (_triggerLoggerRef.triggerType != TriggerType.OnStay) return;
            _triggerLoggerRef.OnCollisionEvent($"Collided with {other.gameObject.name}");
        }

        private void OnTriggerExit(Collider other)
        {
            if (_triggerLoggerRef.triggerType != TriggerType.OnExit) return;
            _triggerLoggerRef.OnCollisionEvent($"Collided with {other.gameObject.name}");
        }
    }
}