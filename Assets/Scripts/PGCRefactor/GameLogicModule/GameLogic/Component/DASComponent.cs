namespace PGCRefactor.GameLogicModule.GameLogic.Component
{
    public class DASComponent
    {
        public const float DasDelay = 0.15f;  // seconds before auto-repeat begins
        public const float ArrRate  = 0.05f;  // auto-repeat interval in seconds

        public float holdTimer;
        public float arrTimer;
        public int   lastDirection;  // -1, 0, or +1
    }
}
