using System;
using Entities;
using UnityEngine;

namespace PGC.Entities.Grid
{
    public class GridEntity
    {
        private Vector2Int gridTopCenter;
        private Vector2Int gridBorder;
        private float gridWorldWidth;
        private Vector3 initWorldPosition;
        
        
        private SquareEntity[,] grid;

        public Vector2Int GridTopCenter {
            get=>gridTopCenter;
            set=>gridTopCenter = value;
        }

        public SquareEntity[,] Grid
        {
            get => grid;
        }
        
        public SquareEntity Get(int x, int y)
        {
            return grid[x, y];
        }
        
        public void Set(int x, int y, SquareEntity squareEntity)
        {
            grid[x, y] = squareEntity;
        }

        public void ClearCell(int x, int y)
        {
            grid[x, y] = null;
        }

        public Vector2Int GridBorder
        {
            get=>gridBorder;
            set=>gridBorder = value;
        }
        
        public float GridWorldWidth => gridWorldWidth;
        public Vector3 InitWorldPosition => initWorldPosition;
        
        public GridEntity(GameContext ctx)
        {
            gridBorder.x = ctx.assetModule.gridSo.gridWidth;
            gridBorder.y = ctx.assetModule.gridSo.gridHeight;
            grid = new SquareEntity[gridBorder.x, gridBorder.y + ctx.assetModule.gridSo.PreHeight];
            gridTopCenter = ctx.assetModule.gridSo.gridTopCenter;
            gridWorldWidth = ctx.assetModule.gridSo.gridWorldWidth;
            initWorldPosition = ctx.assetModule.gridSo.initWorldPosition;
            
            Debug.Log($"initWorldPosition {initWorldPosition}");
        }

        public void LockSquares(GameContext ctx)
        {
            
        }

        public void IsArriveBorder(GameContext ctx)
        {
            
        }

        public void CleanGrid(GameContext ctx)
        {
            
        }

        public bool GridIndexIsNotNull(int x, int y)
        {
            return grid[x, y] != null;
        }

        public Vector3 GetWorldPositionByIndex(Vector2Int index)
        {
            return initWorldPosition + new Vector3(index.x * gridWorldWidth, index.y * gridWorldWidth, 0);
        }
    }
}