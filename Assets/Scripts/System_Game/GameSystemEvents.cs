using System;

namespace PGC {
    
    public class GameSystemEvents {
        
        public Action OnPauseHandle;
        public void OnPauseInvoke() {
            OnPauseHandle.Invoke();
        }

    }
}