using System;
using System.Collections;
using UnityEngine;

namespace oculog.LogSettings
{
    public abstract class LogSettings : ScriptableObject
    {
        public string logId;
        public string description;

        public Action<DataEntry> OnEmit;

        public abstract void Init(GameObject parent);

        public virtual void Tick(){}

        protected void LogIfEnabled<T>(bool enabled, T value, string propertyName)
        {
            if (!enabled) return;

            var entry = new DataEntry($"{logId}-{propertyName}", value.ToString(), Time.time);
            OnEmit.Invoke(entry);
        }

        protected void Log<T>(T value, string propertyName)
        {
            var entry = new DataEntry($"{logId}-{propertyName}", value.ToString(), Time.time);
            OnEmit.Invoke(entry);
        }
    }
}