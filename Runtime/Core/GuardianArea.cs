using System;
using UnityEngine;

namespace oculog.Core
{
    public class GuardianArea : MonoBehaviour
    {
        public Action<DataEntry> OnDataLogged;
        public ELogLevel Level;

        private void OnTriggerEnter(Collider other)
        {
            var hitObj = other.gameObject.name;
            hitObj = other.gameObject.CompareTag("MainCamera") ? "HMD" : hitObj;
            var entry = new DataEntry("guardian", hitObj, Time.time, Level == ELogLevel.Error ? 
                ELogLevel.Warning : ELogLevel.Default);
            OnDataLogged.Invoke(entry);
        }

        private void OnTriggerExit(Collider other)
        {
            var hitObj = other.gameObject.name;
            hitObj = other.gameObject.CompareTag("MainCamera") ? "HMD" : hitObj;
            var entry = new DataEntry("guardian", hitObj, Time.time, Level);
            OnDataLogged.Invoke(entry);
        }
    }
}