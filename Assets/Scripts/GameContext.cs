using System;

namespace PGC {

    // 存储游戏里的所有数据, 以及模块入口
    public class GameContext {

        public UserEntity userEntity;
        public SquareRepository squareRepository;

        public GameContext() { }

    }
}