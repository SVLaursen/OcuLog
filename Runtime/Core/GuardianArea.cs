using System;
using UnityEngine;

namespace oculog.Core
{
    public class GuardianArea : MonoBehaviour
    {
        public Action<DataEntry> OnDataLogged;

        private void OnTriggerEnter(Collider other)
        {
            var hitObj = other.gameObject.name;
            var entry = new DataEntry("guardian", hitObj + " - Entered", Time.time);
            OnDataLogged.Invoke(entry);
        }

        private void OnTriggerExit(Collider other)
        {
            var hitObj = other.gameObject.name;
            var entry = new DataEntry("guardian", hitObj + " - Exited", Time.time, ELogLevel.Error);
            OnDataLogged.Invoke(entry);
        }
    }
}