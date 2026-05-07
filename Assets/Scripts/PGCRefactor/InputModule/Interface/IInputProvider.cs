using PGCRefactor.InputModule.TM;

namespace PGCRefactor.InputModule.Interface
{
    public interface IInputProvider
    {
        InputModel<int> HorizontalMove { get; }
        InputModel<int> VerticalMove   { get; }
        InputModel<int> Rotate         { get; }
        bool            IsChange       { get; }
        bool            IsHold      { get; }

        void Update(float dt);
        void Consume();
    }
}
