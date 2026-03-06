using System;

namespace Custom.Tool
{
    public enum MoveDirection
    {
        Left,
        Right,
        Down
    }
    public enum RotateDirection
    {
        Left,
        Right
    }
    
    public enum SquareMoveStatus
    {
        ReachBorder,
        ReachBottom,
        EnableMove
    }

    public class ConstantTool
    {

        public static int GetRandomInt(int min, int max)
        {
            Random rand = new Random();
            return rand.Next(min, max);
        }
    }

}