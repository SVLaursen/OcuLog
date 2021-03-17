using System;
using System.Collections;

namespace oculog.Targeting
{
    [Serializable]
    public abstract class ApiTarget
    {
        public abstract void Init();

        public abstract void Tick();

        public virtual IEnumerator TrackEveryInterval()
        {
            yield return null;
        }
    }
}