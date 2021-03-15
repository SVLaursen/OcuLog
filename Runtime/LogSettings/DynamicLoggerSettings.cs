using System.Collections;
using UnityEngine;

namespace oculog.LogSettings
{
    [CreateAssetMenu(menuName = "Oculog/Dynamic Logger Settings")]
    public class DynamicLoggerSettings : LogSettings
    {
        public bool useIntervalLogging;
        public float loggingInterval = 0.1f;
        
        public bool trackPosition;
        public bool trackScale;
        public bool trackRotation;

        private GameObject _parent;
        private Transform _parentTransform;
        
        public override void Init(GameObject parent)
        {
            _parent = parent;
            _parentTransform = parent.transform;
        }

        public override void Tick()
        {
            LogIfEnabled(trackPosition, _parentTransform.position, "Position");
            LogIfEnabled(trackScale, _parentTransform.localScale, "Scale");
            LogIfEnabled(trackRotation, _parentTransform.rotation, "Rotation");
        }

        public IEnumerator LogInIntervals()
        {
            while (_parent.activeSelf)
            {
                LogIfEnabled(trackPosition, _parentTransform.position, "Position");
                LogIfEnabled(trackScale, _parentTransform.localScale, "Scale");
                LogIfEnabled(trackRotation, _parentTransform.rotation, "Rotation");

                yield return new WaitForSeconds(loggingInterval);
            }
        }
    }
}