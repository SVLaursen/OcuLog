using System;
using System.Collections;
using oculog.Core;
using UnityEngine;

namespace oculog.LogSettings
{
    [CreateAssetMenu(menuName = "Oculog/Trigger Logger Settings")]
    public class TriggerLoggerSettings : LogSettings
    {
        public TriggerType triggerType;

        public override void Init(GameObject parent)
        {
        }

        public void OnCollisionEvent(string message) => 
            LogIfEnabled(true, message, triggerType.ToString());
    }

    public enum TriggerType
    {
        OnEnter, OnExit, OnStay
    }
}