using System.Collections;
using UnityEngine;

namespace oculog.LogSettings
{
    [CreateAssetMenu(menuName = "Oculog/Static Logger Settings")]
    public class StaticLoggerSettings : LogSettings
    {
        public bool trackPosition;
        public bool trackScale;
        public bool trackRotation;
        
        public override void Init(GameObject parent)
        {
            LogIfEnabled(trackPosition, parent.transform.position, "Position");
            LogIfEnabled(trackScale, parent.transform.localScale, "Scale");
            LogIfEnabled(trackRotation, parent.transform.rotation, "Rotation");
        }
    }
}