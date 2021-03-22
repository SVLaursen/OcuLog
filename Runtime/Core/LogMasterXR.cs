using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace oculog.Core
{
    /*
     * This part of the partial includes all the Unity XR logic.
     * Anything specific to the Unity XR API and its functionality should be included in this part of the partial.
     */
    public partial class LogMaster
    {
        private Camera _hmdCam;
        private GameObject _leftController;
        private GameObject _rightController;

        private Vector3 _previousLeftPosition;
        private Vector3 _previousRightPosition;
        private Vector3 _previousHmdPosition;

        private void CheckAndInitXR()
        {
            if (enableGuardian)
                ActivateGuardianTracking();
            
            if(trackButtonInputs)
                ActivateButtonTracking();
            
            if(trackControllers)
                ActivateControllerTracking();
            
            if(trackHmd)
                ActivateHmdTracking();
        }
        
        private void ActivateGuardianTracking()
        {
            var guardianMesh = GuardianGenerator.GenerateGuardian(guardianIterations, guardianHeight);
            if (!GuardianGenerator.GuardianAvailable) return;
            
            var guardianObj = new GameObject("Guardian");
            var meshFilter = guardianObj.AddComponent<MeshFilter>();
            var meshCollider = guardianObj.AddComponent<MeshCollider>();
            var guardianArea = guardianObj.AddComponent<GuardianArea>();
            
            meshFilter.mesh = guardianMesh;
            meshCollider.sharedMesh = guardianMesh;
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
            guardianArea.Level = ELogLevel.Error;
            guardianArea.OnDataLogged += DataLogger.LogEntry;

            var warningObj = Instantiate(guardianObj, transform.position, Quaternion.identity);
            var warningScale = warningObj.transform.localScale;
            
            warningObj.transform.localScale = new Vector3(warningScale.x - 0.2f, warningScale.y, warningScale.z - 0.2f);
            var warningGuardian = warningObj.GetComponent<GuardianArea>();
            warningGuardian.Level = ELogLevel.Warning;
            warningGuardian.OnDataLogged += DataLogger.LogEntry;
        }

        private void ActivateButtonTracking()
        {
            if (_leftController == null)
                _leftController = GameObject.Find("LeftHand Controller");
            if (_rightController == null)
                _rightController = GameObject.Find("RightHand Controller");

            var leftXR = _leftController.AddComponent<XRInputLogger>();
            var rightXR = _rightController.AddComponent<XRInputLogger>();

            leftXR.ID = "LeftHand";
            rightXR.ID = "RightHand";
            
            leftXR.Initialize(trackLeftTrigger, trackLeftGrip,  trackLeftJoystick, trackLeftBattery, trackLeftFaceButtons);
            rightXR.Initialize(trackRightTrigger, trackRightGrip, trackRightJoystick, trackRightBattery, trackRightFaceButtons);
        }

        private void ActivateControllerTracking()
        {
            if(trackLeftControllerPosition || trackLeftControllerRotation)
                _leftController = GameObject.Find("LeftHand Controller");
            if(trackRightControllerPosition || trackRightControllerRotation)
                _rightController = GameObject.Find("RightHand Controller");

            var leftColl = _leftController.AddComponent<SphereCollider>();
            var rightColl = _rightController.AddComponent<SphereCollider>();
            var leftRb = _leftController.AddComponent<Rigidbody>();
            var rightRb = _rightController.AddComponent<Rigidbody>();

            leftRb.isKinematic = true;
            leftRb.useGravity = false;
            rightRb.isKinematic = true;
            rightRb.useGravity = false;

            leftColl.radius = 0.15f;
            rightColl.radius = 0.15f;

            _previousLeftPosition = _leftController.transform.position;
            _previousRightPosition = _rightController.transform.position;

            if (useIntervalController)
                StartCoroutine(IntervalLogging(controllerInterval, TrackControllers));
            else
                _onTickLog += TrackControllers;
        }

        private void TrackControllers()
        {
            var timeStamp = Time.time;

            if (trackLeftControllerVelocity)
            {
                var currentPosition = _leftController.transform.position;
                var velocity = CalculateVelocity(currentPosition, _previousLeftPosition);
                var entry = new DataEntry("left-controller-velocity", velocity.ToString(), timeStamp);
                
                DataLogger.LogEntry(entry);
                _previousLeftPosition = currentPosition;
            }
            
            if (trackLeftControllerPosition)
            {
                var posVal = _leftController.transform.position.ToString();
                var entry = new DataEntry("left-controller-position", posVal, timeStamp);
                DataLogger.LogEntry(entry);
            }

            if (trackLeftControllerRotation)
            {
                var rotVal = _leftController.transform.rotation.ToString();
                var entry = new DataEntry("left-controller-rotation", rotVal, timeStamp);
                DataLogger.LogEntry(entry);
            }

            if (trackRightControllerVelocity)
            {
                var currentPosition = _rightController.transform.position;
                var velocity = CalculateVelocity(currentPosition, _previousRightPosition);
                var entry = new DataEntry("right-controller-velocity", velocity.ToString(), timeStamp);
                
                DataLogger.LogEntry(entry);
                _previousRightPosition = currentPosition;
            }

            if (trackRightControllerPosition)
            {
                var posVal = _rightController.transform.position.ToString();
                var entry = new DataEntry("right-controller-position", posVal, timeStamp);
                DataLogger.LogEntry(entry);
            }

            if (trackRightControllerRotation)
            {
                var rotVal = _rightController.transform.rotation.ToString();
                var entry = new DataEntry("right-controller-rotation", rotVal, timeStamp);
                DataLogger.LogEntry(entry);
            }
        }

        private void ActivateHmdTracking()
        {
            _hmdCam = Camera.main;
            
            var hmdRb = _hmdCam.gameObject.AddComponent<Rigidbody>();
            hmdRb.isKinematic = true;
            hmdRb.useGravity = false;

            var hmdColl = _hmdCam.gameObject.AddComponent<SphereCollider>();
            hmdColl.radius = 0.1f;

            _previousHmdPosition = _hmdCam.transform.position;

            if (useIntervalHmd)
                StartCoroutine(IntervalLogging(hmdInterval, HmdTracking));
            else
                _onTickLog += HmdTracking;
        }

        private void HmdTracking()
        {
            var timeStamp = Time.time;

            if (trackHmdVelocity)
            {
                var currentPosition = _hmdCam.transform.position;
                var velocity = CalculateVelocity(currentPosition, _previousHmdPosition);
                var entry = new DataEntry("hmd-velocity", velocity.ToString(), timeStamp);
                
                DataLogger.LogEntry(entry);
                _previousHmdPosition = currentPosition;
            }

            if (trackHmdPosition)
            {
                var posEntry = new DataEntry("hmd-position", _hmdCam.transform.position.ToString(), timeStamp);
                DataLogger.LogEntry(posEntry);
            }

            if (!trackHmdRotation) return;
            var rotEntry = new DataEntry("hmd-rotation", _hmdCam.transform.rotation.ToString(), timeStamp);
            DataLogger.LogEntry(rotEntry);
        }

        private Vector3 CalculateVelocity(Vector3 currentPosition, Vector3 previousPosition)
        {
            return (currentPosition - previousPosition) / Time.deltaTime;
        }
    }
}