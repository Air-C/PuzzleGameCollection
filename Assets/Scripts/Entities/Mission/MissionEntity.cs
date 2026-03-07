using System;

namespace PGC {

    public class MissionEntity {

        public int shapeID_cur;
        public ShapeType shapeType_next;

        public float downTimer; // 下落计时器
        public float downInterval; // 下落间隔

        public float gameTimer;
        public float gameTimerMax;

        public MissionEntity() { }

    }

}