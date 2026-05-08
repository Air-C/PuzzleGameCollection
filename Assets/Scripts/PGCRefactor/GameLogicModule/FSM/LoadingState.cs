using PGCRefactor.GameLogicModule.FSM.Enum;
using PGCRefactor.GameLogicModule.FSM.Interface;
using PGCRefactor.PresenterLayer.UIModule;
using PGCRefactor.PresenterLayer.UIModule.Interface;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.FSM
{
    public class LoadingState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly ILoadingPageUI _loadingPageUI;

        public LoadingState(IStateMachine stateMachine, ILoadingPageUI loadingPageUI)
        {
            _stateMachine = stateMachine;
            _loadingPageUI = loadingPageUI;
        }

        public void Enter()
        {
            
        }

        public void Update()
        {
            _loadingPageUI.SetLoadingPageSlider();
            if(_loadingPageUI.GetLoadingProcess() >= 100)
            {
                _stateMachine.ChangeState(GameStateEnum.MainMenu);
            }
        }

        public void Exit()
        {
            _loadingPageUI.HideLoadingPage();
        }
    }
}