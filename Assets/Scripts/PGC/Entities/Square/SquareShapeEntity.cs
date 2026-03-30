using System.Collections.Generic;
using Custom.Tool;
using Entities.SO;
using PGC;
using PGC.Enum;
using UnityEngine;

namespace Entities
{
    public class SquareShapeEntity
    {
        private List<SquareEntity> squares = new ();
        public string Name{get;set;}
        public ShapeRotateStatusEnum RotateStatus{get;set;}
        public Vector2Int[] waitForExcWallKick = null;
        public ShapeTypeEnum ShapeType{get;set;}
        private readonly Dictionary<(ShapeRotateStatusEnum from, ShapeRotateStatusEnum to), Vector2Int[]> WallKickJLSTZ =
            new()
            {
                // 0 → R
                {(ShapeRotateStatusEnum.Spawn,ShapeRotateStatusEnum.Right), new [] { new Vector2Int(0,0), new(-1,0), new(-1,1), new(0,-2), new(-1,-2) }},
                // R → 0
                {(ShapeRotateStatusEnum.Right,ShapeRotateStatusEnum.Spawn), new [] { new Vector2Int(0,0), new(1,0), new(1,-1), new(0,2), new(1,2) }},

                // R → 2
                {(ShapeRotateStatusEnum.Right,ShapeRotateStatusEnum.Reverse), new [] { new Vector2Int(0,0), new(1,0), new(1,-1), new(0,2), new(1,2) }},
                // 2 → R
                {(ShapeRotateStatusEnum.Reverse,ShapeRotateStatusEnum.Right), new [] { new Vector2Int(0,0), new(-1,0), new(-1,1), new(0,-2), new(-1,-2) }},

                // 2 → L
                {(ShapeRotateStatusEnum.Reverse,ShapeRotateStatusEnum.Left), new [] { new Vector2Int(0,0), new(1,0), new(1,1), new(0,-2), new(1,-2) }},
                // L → 2
                {(ShapeRotateStatusEnum.Left,ShapeRotateStatusEnum.Reverse), new [] { new Vector2Int(0,0), new(-1,0), new(-1,-1), new(0,2), new(-1,2) }},

                // L → 0
                {(ShapeRotateStatusEnum.Left,ShapeRotateStatusEnum.Spawn), new [] { new Vector2Int(0,0), new(-1,0), new(-1,-1), new(0,2), new(-1,2) }},
                // 0 → L
                {(ShapeRotateStatusEnum.Spawn,ShapeRotateStatusEnum.Left), new [] { new Vector2Int(0,0), new(1,0), new(1,1), new(0,-2), new(1,-2) }},
            };
        
        private readonly Dictionary<(ShapeRotateStatusEnum from, ShapeRotateStatusEnum to), Vector2Int[]> WallKickI =
            new()
            {
                // 0 → R
                {(ShapeRotateStatusEnum.Spawn,ShapeRotateStatusEnum.Right), new [] { new Vector2Int(0,0), new(-2,0), new(1,0), new(-2,-1), new(1,2) }},
                // R → 0
                {(ShapeRotateStatusEnum.Right,ShapeRotateStatusEnum.Spawn), new [] { new Vector2Int(0,0), new(2,0), new(-1,0), new(2,1), new(-1,-2) }},

                // R → 2
                {(ShapeRotateStatusEnum.Right,ShapeRotateStatusEnum.Reverse), new [] { new Vector2Int(0,0), new(-1,0), new(2,0), new(-1,2), new(2,-1) }},
                // 2 → R
                {(ShapeRotateStatusEnum.Reverse,ShapeRotateStatusEnum.Right), new [] { new Vector2Int(0,0), new(1,0), new(-2,0), new(1,-2), new(-2,1) }},

                // 2 → L
                {(ShapeRotateStatusEnum.Reverse,ShapeRotateStatusEnum.Left), new [] { new Vector2Int(0,0), new(2,0), new(-1,0), new(2,1), new(-1,-2) }},
                // L → 2
                {(ShapeRotateStatusEnum.Left,ShapeRotateStatusEnum.Reverse), new [] { new Vector2Int(0,0), new(-2,0), new(1,0), new(-2,-1), new(1,2) }},

                // L → 0
                {(ShapeRotateStatusEnum.Left,ShapeRotateStatusEnum.Spawn), new [] { new Vector2Int(0,0), new(1,0), new(-2,0), new(1,-2), new(-2,1) }},
                // 0 → L
                {(ShapeRotateStatusEnum.Spawn,ShapeRotateStatusEnum.Left), new [] { new Vector2Int(0,0), new(-1,0), new(2,0), new(-1,2), new(2,-1) }},
            };

        public ShapeRotateStatusEnum RotateStatusMoveNext()
        {
            RotateStatus = ConstantTool.GetEnumNext(RotateStatus);
            return RotateStatus;
        }

        public void AddSquare(SquareEntity square)
        {
            squares.Add(square);
        }

        public void AddSquaresFirst(SquareEntity square)
        {
            squares.Insert(0,square);
        }

        public List<SquareEntity> GetSquares()
        {
            return squares;
        }

        public void ClearSquares()
        {
            RotateStatus = ShapeRotateStatusEnum.Spawn;
            waitForExcWallKick = null;
            squares.Clear();
        }
        
        public Vector2Int[] GetWallKickRules(ShapeRotateStatusEnum from, ShapeRotateStatusEnum to, ShapeTypeEnum shapeType)
        {
            if (shapeType == ShapeTypeEnum.IShape)
            {
                if (WallKickI.TryGetValue((from, to), out var rules))
                {
                    return rules;
                }
            }
            else
            {
                if (WallKickJLSTZ.TryGetValue((from, to), out var rules))
                {
                    return rules;
                }
            }
            return null;
        }
        
        public void SetWallKick(Vector2Int[] wallKick)
        {
            waitForExcWallKick = wallKick;
        }
        
        public void ClearWallKick()
        {
            waitForExcWallKick = null;
        }
    }
}