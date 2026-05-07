using PGCRefactor.AudioModule;
using PGCRefactor.GameLogicModule.FSM;
using PGCRefactor.ModuleAsset;
using PGCRefactor.UIModule;

namespace PGCRefactor
{
    public class GameContext
    {
        
        
        
        public UIManager uiManager;
        public AssetsLoadModule assetsLoadModule;
        public AudioManager audioManager;
        public GameFsm gameFsm;
        
    }
}