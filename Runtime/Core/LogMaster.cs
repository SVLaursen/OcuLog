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
        public LogMasterSettings settings;
        
        //Unity XR Specifics
        public bool useXR;
        
        //Guardian
        public bool enableGuardian;
        public int guardianIterations;
        public float guardianHeight;
        
        //Controllers
        public XRControllerData leftController;
        public XRControllerData rightController;

        //HMD
        public bool trackHmd;

        public bool trackHmdVelocity;
        public bool trackHmdPosition;
        public bool trackHmdRotation;

        //Actions
        private Action _onTickLog;
    }
}