using PGCRefactor.GameLogicModule.FSM.Enum;

namespace PGCRefactor.GameLogicModule.FSM.Interface
{
    public interface IStateMachine
    {
        
        public void ChangeState(GameStateEnum next) {}
        
        public void Update() {}
        
    }
}