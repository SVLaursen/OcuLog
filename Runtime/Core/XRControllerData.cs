namespace oculog.Core
{
    [System.Serializable]
    public struct XRControllerData
    {
        public bool logButtonInputs;
        public bool trigger, grip, joystick, faceButtons;

        public bool logMovement;
        public bool position, rotation, velocity, acceleration;
    }
}