using PGCRefactor.GameLogicModule.FSM.Enum;
using PGCRefactor.GameLogicModule.FSM.Interface;
using PGCRefactor.PresenterLayer.UIModule;
using PGCRefactor.PresenterLayer.UIModule.Interface;

namespace PGCRefactor.GameLogicModule.FSM
{
    public class GameFsm : IStateMachine
    {
        private IState _currentState;
        private readonly ILoadingPageUI _loadingPageUI;
        private readonly IMainMenuUI _mainMenuUI;
        private GameSessionContext _sessionContext;

        public GameFsm(ILoadingPageUI loadingPageUI, IMainMenuUI mainMenuUI)
        {
            _loadingPageUI = loadingPageUI;
            _mainMenuUI = mainMenuUI;
        }

        // Called by GameController before ChangeState(GameLoop)
        public void SetSessionContext(GameSessionContext ctx)
        {
            _sessionContext = ctx;
        }

        public void ChangeState(GameStateEnum next)
        {
            _currentState?.Exit();
            _currentState = GenerateState(next);
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState.Update();
        }

        private IState GenerateState(GameStateEnum next)
        {
            switch (next)
            {
                case GameStateEnum.Loading:  return new LoadingState(this, _loadingPageUI);
                case GameStateEnum.MainMenu: return new MainMenuState(this, _mainMenuUI);
                case GameStateEnum.GameLoop: return new GameLoopState(this, _sessionContext);
                case GameStateEnum.Pause:    return new PauseState(this);
                case GameStateEnum.GameOver: return new GameOverState(this);
                default:                     return null;
            }
        }
    }
}