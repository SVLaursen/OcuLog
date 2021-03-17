using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace oculog.Targeting
{
    [Serializable]
    public class OculusSettings : ApiTarget
    {
        #region Inspector Variables
        //Guardian interaction
        public bool trackGuardian;
        
        //Button presses
        public bool trackTriggerButtonInput;
        public bool trackGripButtonInput;
        public bool trackFaceButtonInput;
        public bool trackJoystickInput;
        
        //Controller (Movement) tracking
        public bool useIntervalTrackingController;
        public float controllerInterval;
        
        public bool trackControllerRotation;
        public float controllerRotationThreshold;
        
        public bool trackControllerPosition;
        public float controllerPositionThreshold;

        //Head Tracking
        public bool useIntervalTrackingHmd;
        public float hmdInterval;
        
        public bool trackHmdPosition;
        public float hmdPositionThreshold;

        public bool trackHmdRotation;
        public float hmdRotationThreshold;
        #endregion

        private Action _methodsToTick;
        private Action _methodsToInterval;
        private Action<Vector3> _guardianNodeHit;

        private List<Vector3> _guardianNodes;
        
        public override void Init()
        {
            //Get boundary area if guardian is enabled and set it up
            if (trackGuardian)
                SetupGuardianTracking();
        }

        public override void Tick()
        {
            _methodsToTick?.Invoke();
        }

        public override IEnumerator TrackEveryInterval()
        {
            throw new NotImplementedException();
        }

        private void TrackHmd()
        {
            throw new NotImplementedException();
        }

        private void TrackController()
        {
            throw new NotImplementedException();
        }

        private void SetupGuardianTracking()
        {
            _guardianNodes = GetBoundaryPoints();
            
            if(_guardianNodes.Count <= 0)
            {
                Debug.LogError( "Oculog could not find Oculus Guardian points and therefore will " +
                                "not track this feature");
                return;
            }
            
            _methodsToTick += CheckGuardianNodes;
            _guardianNodeHit += OnGuardianNodeHit;
        }

        private void OnGuardianNodeHit(Vector3 position)
        {
            var entry = new DataEntry("guardian", position.ToString(), Time.time,
                ELogLevel.Error);
            DataLogger.LogEntry(entry);
        }

        private void CheckGuardianNodes()
        {
            //foreach (var entry in _guardianNodes)
            //{
                //Perform checks on the hmd and controllers to see if their position is outside?
                //Or maybe move this function to something else? idk
            //}
        }

        private List<Vector3> GetBoundaryPoints()
        {
            /*var configured = OVRManager.boundary.GetConfigured();
            var result = new List<Vector3>();
            
            if (!configured) return result;
            
            var boundaryPoints = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.OuterBoundary);
            result = boundaryPoints.ToList();
            return result;*/

            var inputSubsystem = new List<XRInputSubsystem>();
            SubsystemManager.GetInstances<XRInputSubsystem>(inputSubsystem);

            if (inputSubsystem.Count <= 0) return new List<Vector3>();
            var boundary = new List<Vector3>();

            return inputSubsystem[0].TryGetBoundaryPoints(boundary) ? boundary : null;
        }
    }
}