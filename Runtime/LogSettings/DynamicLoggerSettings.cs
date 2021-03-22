using System.Collections;
using UnityEngine;

namespace oculog.LogSettings
{
    [CreateAssetMenu(menuName = "Oculog/Dynamic Logger Settings")]
    public class DynamicLoggerSettings : LogSettings
    {
        public bool useIntervalLogging;
        public float loggingInterval = 0.1f;

        public bool trackVelocity;
        public bool trackPosition;
        public bool trackScale;
        public bool trackRotation;

        private GameObject _parent;
        private Transform _parentTransform;
        private Vector3 _previousPosition;
        
        public override void Init(GameObject parent)
        {
            _parent = parent;
            _parentTransform = parent.transform;
            _previousPosition = _parentTransform.position;
        }

        public override void Tick()
        {
            if (trackVelocity)
            {
                var currentPosition = _parentTransform.position;
                Log(GetVelocity(currentPosition), "Velocity");
                _previousPosition = currentPosition;
            }
            
            LogIfEnabled(trackPosition, _parentTransform.position, "Position");
            LogIfEnabled(trackScale, _parentTransform.localScale, "Scale");
            LogIfEnabled(trackRotation, _parentTransform.rotation, "Rotation");
        }

        public IEnumerator LogInIntervals()
        {
            while (_parent.activeSelf)
            {
                if (trackVelocity)
                {
                    var currentPosition = _parentTransform.position;
                    Log(GetVelocity(currentPosition), "Velocity");
                    _previousPosition = currentPosition;
                }
                
                LogIfEnabled(trackPosition, _parentTransform.position, "Position");
                LogIfEnabled(trackScale, _parentTransform.localScale, "Scale");
                LogIfEnabled(trackRotation, _parentTransform.rotation, "Rotation");

                yield return new WaitForSeconds(loggingInterval);
            }
        }

        private Vector3 GetVelocity(Vector3 currentPosition)
        {
            return (currentPosition - _previousPosition) / Time.deltaTime;
        }
    }
}