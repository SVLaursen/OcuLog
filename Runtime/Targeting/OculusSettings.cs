using System;
using UnityEngine;

namespace oculog.Targeting
{
    [Serializable]
    public class OculusSettings : ApiTarget
    {
        [SerializeField] public bool testOculus;
        
        public override void Init()
        {
            throw new System.NotImplementedException();
        }
    }
}