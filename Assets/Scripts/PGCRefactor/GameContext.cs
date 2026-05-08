using PGCRefactor.PresenterLayer.AudioModule;
using PGCRefactor.GameLogicModule.FSM;
using PGCRefactor.InfrastructureLayer.AssetsLoadModule;
using PGCRefactor.PresenterLayer.UIModule;

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