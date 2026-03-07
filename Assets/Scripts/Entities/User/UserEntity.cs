using System;

namespace PGC {

    public class UserEntity {
        
        public int idRecord_Square;

        public UserEntity() { }

        public int ID_Square() {
            return ++idRecord_Square;
        }
    }
}