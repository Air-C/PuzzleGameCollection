using PGCRefactor.InputModule.Enum;

namespace PGCRefactor.InputModule.TM
{
    public struct InputModel<T> where T : struct
    {
        public InputStatus status;
        public T           value;
    }
}
