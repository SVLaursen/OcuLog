using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using CommonUsages = UnityEngine.XR.CommonUsages;

namespace oculog.Core
{
    public class XRControllerLogger : MonoBehaviour
    {
        public string ID { get; set; }

        private XRControllerData _settings;

        private bool _prevTrigger, _prevGrip, _prevJoystickClick, _prevABtn, _prevBBtn, _prevTracking;
        
        private XRController _controller;

        private Action _logOnTick;

        public void Initialize(XRControllerData settings)
        {
            if (settings.logButtonInputs)
            {
                if (settings.trigger)
                    _logOnTick += TrackTrigger;
                if (settings.grip)
                    _logOnTick += TrackGrip;
                if (settings.joystick)
                    _logOnTick += TrackJoystick;
                if (settings.faceButtons)
                    _logOnTick += TrackFaceButtons;
            }

            if (settings.logMovement)
            {
                if (settings.position)
                    _logOnTick += TrackControllerPosition;
                if (settings.rotation)
                    _logOnTick += TrackControllerRotation;
                if (settings.velocity)
                    _logOnTick += TrackControllerVelocity;
                if (settings.acceleration)
                    _logOnTick += TrackControllerAcceleration;
            }

            _logOnTick += LookForTrackingLoss;
            
            _settings = settings;
            _controller = GetComponent<XRController>();
        }

        private void Awake()
        {
            var coll = gameObject.AddComponent<SphereCollider>();
            var rb = gameObject.AddComponent<Rigidbody>();

            rb.isKinematic = true;
            rb.useGravity = false;

            coll.radius = 0.15f;
        }
        
        private void Update() => _logOnTick?.Invoke();

        private void OnDisable()
        {
            if (_settings.logButtonInputs)
            {
                if (_settings.trigger)
                    _logOnTick -= TrackTrigger;
                if (_settings.grip)
                    _logOnTick -= TrackGrip;
                if (_settings.joystick)
                    _logOnTick -= TrackJoystick;
                if (_settings.faceButtons)
                    _logOnTick -= TrackFaceButtons;
            }

            _logOnTick -= LookForTrackingLoss;

            if (!_settings.logMovement) return;
            if (_settings.position)
                _logOnTick -= TrackControllerPosition;
            if (_settings.rotation)
                _logOnTick -= TrackControllerRotation;
            if (_settings.velocity)
                _logOnTick -= TrackControllerVelocity;
            if (_settings.acceleration)
                _logOnTick -= TrackControllerAcceleration;
        }

        private void TrackTrigger()
        {
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerVal)) return;
            if (!_prevTrigger && !triggerVal || _prevTrigger && triggerVal) return;
            
            var value = triggerVal ? "pressed" : "released";
            _prevTrigger = triggerVal;
            
            var entry = new DataEntry($"{ID}-trigger", value, Time.time);
            DataLogger.LogEntry(entry);
        }

        private void TrackGrip()
        {
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool grip)) return;
            if (!_prevGrip && !grip || _prevGrip && grip) return;

            var value = grip ? "pressed" : "released";
            _prevGrip = grip;
            
            var entry = new DataEntry($"{ID}-grip", value, Time.time);
            DataLogger.LogEntry(entry);
        }

        private void TrackJoystick()
        {
            if (_controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var value))
            {
                var entry = new DataEntry($"{ID}-joystick", value.ToString(), Time.time);
                DataLogger.LogEntry(entry);
            }

            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out var clicked)) return;
            {
                if (!_prevJoystickClick && !clicked || _prevJoystickClick && clicked) return;

                var clickVal = clicked ? "pressed" : "released";
                _prevJoystickClick = clicked;
                
                var entry = new DataEntry($"{ID}-joystick-press", clickVal, Time.time);
                DataLogger.LogEntry(entry);
            }
        }

        private void TrackFaceButtons()
        {
            if (_controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out var aClick))
            {
                if (!_prevABtn && !aClick || _prevABtn && aClick) return;

                var value = aClick ? "pressed" : "released";
                _prevABtn = aClick;
                
                var entry = new DataEntry($"{ID}-A-button", value, Time.time);
                DataLogger.LogEntry(entry);
            }

            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out var bClick)) return;
            {
                if (!_prevBBtn && !bClick || _prevBBtn && bClick) return;
                
                var value = bClick ? "pressed" : "released";
                _prevBBtn = bClick;
                
                var entry = new DataEntry($"{ID}-B-button", value, Time.time);
                DataLogger.LogEntry(entry);
            }
        }

        private void TrackControllerPosition()
        {
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var value)) return;
            var entry = new DataEntry($"{ID}-position", value.ToString(), Time.time);
            DataLogger.LogEntry(entry);
        }

        private void TrackControllerRotation()
        {
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out var value)) return;
            var entry = new DataEntry($"{ID}-rotation", value.ToString(), Time.time);
            DataLogger.LogEntry(entry);
        }

        private void TrackControllerVelocity()
        {
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out var value)) return;
            var entry = new DataEntry($"{ID}-velocity", value.ToString(), Time.time);
            DataLogger.LogEntry(entry);
        }

        private void TrackControllerAcceleration()
        {
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceAcceleration, out var value)) return;
            var entry = new DataEntry($"{ID}-acceleration", value.ToString(), Time.time);
            DataLogger.LogEntry(entry);
        }

        private void LookForTrackingLoss()
        {
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.isTracked, out var status)) return;
            if (_prevTracking && status || !_prevTracking && !status) return;
            
            var value = status ? "tracking" : "lost";
            var logLevel = status ? ELogLevel.Default : ELogLevel.Error;
            _prevTracking = status;
            
            var entry = new DataEntry($"{ID}-trackin-loss", value, Time.time, logLevel);
            DataLogger.LogEntry(entry);
        }
    }
}