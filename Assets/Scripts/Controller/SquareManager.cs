using System.Collections.Generic;
using Custom.Tool;

namespace PGC.Controller {

    public class SquareManager {

        readonly string[] squareNames = { "PurpleDotSquare", "OrangeSquare", "BlueStarSquare", "PurpleSquare", "BlueSquare", "GreenSquare", "LightBlueSquare" };
        string path = "Assets/Prefab/";
        string suffix = ".prefab";

        string GetRandomSquareName() {
            int randIndex = UnityEngine.Random.Range(0, squareNames.Length);
            return path + squareNames[randIndex] + suffix;
        }

        public void Move(SquareEntity square, MoveDirection shift) {
            Int2 index = square.GridIndex;
            switch (shift) {
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

        public void Roate(SquareEntity square, RotateDirection turn) {
            Int2 index = square.GridIndex;
            switch (turn) {
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
    }
}