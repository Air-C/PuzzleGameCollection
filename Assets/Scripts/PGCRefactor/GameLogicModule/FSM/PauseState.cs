using PGCRefactor.GameLogicModule.FSM.Interface;

namespace PGCRefactor.GameLogicModule.FSM
{
    public class PauseState : IState
    {
        private IStateMachine stateMachine;
        
        public PauseState(IStateMachine stateMachine)
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