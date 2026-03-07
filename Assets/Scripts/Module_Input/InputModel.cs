using System;

namespace PGC.Module_Input {

    public struct InputModel<T> where T : struct {

        public InputStatus status;
        public T value;

    }

}