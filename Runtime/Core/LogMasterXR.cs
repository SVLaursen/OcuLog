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

        private Vector3 _previousHmdPosition;

        private void CheckAndInitXR()
        {
            if (enableGuardian)
                ActivateGuardianTracking();

            if(trackHmd)
                ActivateHmdTracking();
            
            ActivateControllers();
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

        private void ActivateControllers()
        {
            if(leftController.logMovement || leftController.logButtonInputs)
                ActivateController("LeftHand Controller", "LeftController", leftController);
            
            if(rightController.logMovement || rightController.logButtonInputs)
                ActivateController("RightHand Controller", "RightController", rightController);
        }

        private void ActivateController(string controllerName, string id, XRControllerData data)
        {
            var controller = GameObject.Find(controllerName);
            var controllerXR = controller.AddComponent<XRControllerLogger>();
            controllerXR.ID = id;
            controllerXR.Initialize(data);
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