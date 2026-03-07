using System.Collections.Generic;
using System.Linq;
using Custom.Tool;
using UnityEngine;

namespace PGC.Controller {

    public class GridManager {

        public const string Hold = "Hold";
        public const string Current = "Current";

        public SquareEntity[,] grid = new SquareEntity[10, 18];
        public readonly Int2 gridTopCenter = new(4, 14);
        public readonly Int2 gridBorder = new(10, 14);
        public readonly Int2 gridHoldCenter = new(8, 16);

        public GridManager() {

        }

        public SquareEntity[,] Grid {
            get => grid;
            set => grid = value;
        }

        public SquareMoveStatus IsEnableMove(List<SquareEntity> squares, MoveDirection moveDirection) {
            foreach (SquareEntity square in squares) {
                Vector2Int newGridIndex = square.GridIndex;
                switch (moveDirection) {
                    case MoveDirection.Left:
                        newGridIndex.x -= 1;
                        break;
                    case MoveDirection.Right:
                        newGridIndex.x += 1;
                        break;
                    case MoveDirection.Down:
                        newGridIndex.y -= 1;
                        break;
                    default:
                        break;
                }

                if (newGridIndex.x < 0 || newGridIndex.x >= grid.GetLength(0)) {
                    return SquareMoveStatus.ReachBorder;
                }

                if (newGridIndex.y < 0 || grid[newGridIndex.x, newGridIndex.y] != null) {
                    return SquareMoveStatus.ReachBottom;
                }

            }
            return SquareMoveStatus.EnableMove;
        }

        public SquareMoveStatus IsEnableRotate(List<SquareEntity> squares, RotateDirection direction) {
            foreach (SquareEntity square in squares) {
                Vector2Int newGridIndex = square.GridIndex;
                switch (direction) {
                    case RotateDirection.Left:
                        newGridIndex = new Vector2Int(-square.GridIndex.y, square.GridIndex.x);
                        break;
                    case RotateDirection.Right:
                        newGridIndex = new Vector2Int(square.GridIndex.y, -square.GridIndex.x);
                        break;
                    default:
                        break;
                }

                if (newGridIndex.x < 0 || newGridIndex.x >= grid.GetLength(0)) {
                    return SquareMoveStatus.ReachBorder;
                }

                if (newGridIndex.y < 0 || grid[newGridIndex.x, newGridIndex.y] != null) {
                    return SquareMoveStatus.ReachBottom;
                }

            }
            return SquareMoveStatus.EnableMove;
        }

        public void SaveSquare(List<SquareEntity> squares) {
            foreach (var square in squares) {
                if (grid[square.GridIndex.x, square.GridIndex.y] == null) {
                    grid[square.GridIndex.x, square.GridIndex.y] = square;
                }
            }
        }

        public bool IsRowFull(int row) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                if (grid[x, row] == null) {
                    return false;
                }
            }
            return true;
        }

        public void ClearRow(HashSet<int> rows) {
            int minRow = rows.Min();
            foreach (var row in rows) {
                for (int x = 0; x < grid.GetLength(0); x++) {
                    grid[x, row] = null;
                }
            }
            // 棋盘数据下移动
            for (int x = 0; x < grid.GetLength(0); x++) {
                for (int y = minRow; y < grid.GetLength(1); y++) {
                    grid[x, y] = y + rows.Count < grid.GetLength(1) ? grid[x, y + rows.Count] : null;
                }
            }
        }
    }
}