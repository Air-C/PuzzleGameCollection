using PGC.Enum;

namespace PGC.ModuleInput
{
    public struct InputModel<T> where T : struct
    {
        public InputStatus status;
        public T value;
        
    }
}