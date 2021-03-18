using System;
using System.Collections.Generic;
using oculog.LogSettings;
using UnityEngine;

namespace oculog.Core
{
    /*
     * This part of the partial includes the base properties and inspector values.
     * All global variables for the class must be setup in this part of the partial.
     */
    public partial class LogMaster : MonoBehaviour
    {
        //General Settings (Fps tracking, etc.)
        [SerializeField] private LogMasterSettings settings;
        
        //Unity XR Specifics
        public bool useXR;
        
        //Guardian
        public bool enableGuardian;
        public int guardianIterations;
        public float guardianHeight;
        
        //Button Inputs
        public bool trackButtonInputs;

        public bool trackLeftTrigger;
        public bool trackRightTrigger;

        public bool trackLeftGrip;
        public bool trackRightGrip;

        public bool trackLeftFaceButtons;
        public bool trackRightFaceButtons;

        public bool trackLeftJoystick;
        public bool trackRightJoystick;

        //Controllers
        public bool trackControllers;

        public bool useIntervalController;
        public float controllerInterval;

        public bool trackLeftControllerRotation;
        public bool trackRightControllerRotation;

        public bool trackLeftControllerPosition;
        public bool trackRightControllerPosition;

        //HMD
        public bool trackHmd;

        public bool useIntervalHmd;
        public float hmdInterval;

        public bool trackHmdPosition;
        public bool trackHmdRotation;

        //Actions
        private Action _onTickLog;
    }
}