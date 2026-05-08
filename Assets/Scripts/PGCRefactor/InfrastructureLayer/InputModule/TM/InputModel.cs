using PGCRefactor.AssetsLoadModule.InputModule.Enum;

namespace PGCRefactor.AssetsLoadModule.InputModule.TM
{
    public struct InputModel<T> where T : struct
    {
        public InputStatus status;
        public T           value;
    }
}
