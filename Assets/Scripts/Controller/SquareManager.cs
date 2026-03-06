using System.Collections.Generic;
using Custom.Tool;

namespace Controller
{
    public class SquareManager
    {
        private readonly string[] squareNames = {"PurpleDotSquare", "OrangeSquare", "BlueStarSquare", "PurpleSquare", "BlueSquare", "GreenSquare", "LightBlueSquare"};
        private string path = "Assets/Prefab/";
        private string suffix = ".prefab";
        
        
        
        private string GetSquareName()
        {
            int randIndex = UnityEngine.Random.Range(0, squareNames.Length);
            return path+squareNames[randIndex]+suffix;
        }


        public void Move(Square square, MoveDirection shift)
        {
            Int2 index = square.GridIndex;
            switch (shift)
            {
                case MoveDirection.Left:
                    index.x--;
                    break;
                case MoveDirection.Right:
                    index.x++;
                    break;
                case MoveDirection.Down:
                    index.y--;
                    break;
                default:
                    break;
            }
            square.GridIndex = index;
        }
        
        public void Roate(Square square, RotateDirection turn)
        {
            Int2 index = square.GridIndex;
            switch (turn)
            {
                case RotateDirection.Left:
                    index = new Int2(-index.y, index.x);
                    break;
                case RotateDirection.Right:
                    index = new Int2(index.y, -index.x);
                    break;
                default:
                    break;
            }
            square.GridIndex = index;
        }
        
        public List<Square> GenerateSquaresForShape(Int2[] squareIndex, Int2 initGridIndex)
        {
            List<Square> squareList = new List<Square>();

            foreach (var index in squareIndex)
            {
                Square square = new Square()
                {
                    GridIndex = index + initGridIndex,
                    Name = GetSquareName(),
                };
                squareList.Add(square);
            }

            return squareList;
        }
        
        public List<Square> ChangeSquaresForShape(List<Square> squares, Int2 initGridIndex)
        {
            List<Square> squareList = new List<Square>();

            foreach (var square in squares)
            {
                square.GridIndex += initGridIndex;
            }
            return squareList;
        }
    }
}