using PGCRefactor.GameLogicModule.FSM.Enum;
using PGCRefactor.GameLogicModule.FSM.Interface;
using PGCRefactor.UIModule;
using PGCRefactor.UIModule.DipInterface;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.FSM
{
    public class MainMenuState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IMainMenuUI _mainMenuUI;

        public MainMenuState(IStateMachine stateMachine, IMainMenuUI mainMenuUI)
        {
            _stateMachine = stateMachine;
            _mainMenuUI = mainMenuUI;
        }
        
        public void Enter()
        {
            Debug.Log("Enter Main Menu State");
            _mainMenuUI.SetStartGameAction(() =>
            {
                Debug.Log(" Start Game Button Clicked");
                _stateMachine.ChangeState(GameStateEnum.GameLoop);
            });
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            Debug.Log("Exit Main Menu State");

            _mainMenuUI.HideMainMenu();
        }

    }
}