using oculog.Targeting;
using UnityEngine;

namespace oculog.LogSettings
{
    [CreateAssetMenu(menuName = "Oculog/LogMaster Settings")]
    public class LogMasterSettings : LogSettings
    {
        [SerializeField] private ETargetApi platform;
        [SerializeField] private ApiTarget targetApiSettings = default;
        
        public EExportType exportType;

        
        public override void Init()
        {
            throw new System.NotImplementedException();
        }

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}