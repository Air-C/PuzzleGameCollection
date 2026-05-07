using PGCRefactor.GameLogicModule.FSM.Interface;

namespace PGCRefactor.GameLogicModule.FSM
{
    public class GameOverState : IState
    {
        private IStateMachine stateMachine;

        public GameOverState(IStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}