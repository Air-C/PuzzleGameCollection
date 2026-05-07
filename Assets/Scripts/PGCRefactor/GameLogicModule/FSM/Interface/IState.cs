namespace PGCRefactor.GameLogicModule.FSM.Interface
{
    public interface IState
    {
        public void Enter();

        public void Tick(float deltaTime){}
        public void Update();

        public void Exit();
    }
}