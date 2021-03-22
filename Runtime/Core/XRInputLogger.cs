using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace oculog.Core
{
    public class XRInputLogger : MonoBehaviour
    {
        public string ID { get; set; }

        private bool _trigger, _grip, _joystick, _battery, _faceButtons;
        
        private XRController _controller;

        private Action _logOnTick;

        public void Initialize(bool trigger, bool grip,  bool joystick, bool battery, bool faceButtons)
        {
            if (trigger)
                _logOnTick += TrackTrigger;
            if (grip)
                _logOnTick += TrackGrip;
            if (joystick)
                _logOnTick += TrackJoystick;
            if (battery)
                _logOnTick += TrackBattery;
            if (faceButtons)
                _logOnTick += TrackFaceButtons;

            _trigger = trigger;
            _grip = grip;
            _joystick = joystick;
            _battery = battery;
            _faceButtons = faceButtons;
            
            _controller = GetComponent<XRController>();
        }

        private void Update() => _logOnTick?.Invoke();

        private void OnDisable()
        {
            if (_grip)
                _logOnTick -= TrackGrip;
            if (_trigger)
                _logOnTick -= TrackTrigger;
            if (_joystick)
                _logOnTick -= TrackJoystick;
            if (_battery)
                _logOnTick -= TrackBattery;
            if (_faceButtons)
                _logOnTick -= TrackFaceButtons;
        }

        private void TrackTrigger()
        {
            Debug.Log(_controller);
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger)) return;
            var entry = new DataEntry($"{ID}-trigger", "pressed", Time.time);
            DataLogger.LogEntry(entry);
        }

        private void TrackGrip()
        {
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool grip)) return;
            var entry = new DataEntry($"{ID}-grip", "pressed", Time.time);
            DataLogger.LogEntry(entry);
        }

        private void TrackBattery()
        {
            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.batteryLevel, out var value)) return;
            var entry = new DataEntry($"{ID}-battery", $"{value.ToString()}%", Time.time);
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
                var entry = new DataEntry($"{ID}-joystick-press", "pressed", Time.time);
                DataLogger.LogEntry(entry);
            }
        }

        private void TrackFaceButtons()
        {
            if (_controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out var click))
            {
                var entry = new DataEntry($"{ID}-A-button", "pressed", Time.time);
                DataLogger.LogEntry(entry);
            }

            if (!_controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out var value)) return;
            {
                var entry = new DataEntry($"{ID}-B-button", "pressed", Time.time);
                DataLogger.LogEntry(entry);
            }
        }
    }
}