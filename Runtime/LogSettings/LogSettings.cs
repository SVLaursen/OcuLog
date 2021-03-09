using System;
using UnityEngine;

namespace oculog.LogSettings
{
    public abstract class LogSettings : ScriptableObject
    {
        public string logId;
        public string description;

        public Action<DataEntry> OnEmit;

        public abstract void Init();

        public abstract void Tick();
    }
}