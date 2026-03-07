using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using Custom.Tool;
using Unity.VisualScripting;
using PGC.Controller;

namespace PGC.MainEntry {

    public class GameSystem : MonoBehaviour {

        // ==== Context ====
        GameContext ctx;

        // ==== AssetModule ====
        AssetModule assetModule;

        // ==== Entity ====
        UserEntity userEntity;
        SquareRepository squareRepository;

        // 管理当前下落中的方块
        private List<SquareEntity> squaresOfCurrentShape = new();
        private List<SquareEntity> squaresOfHoldShape = new();
        const float MoveInterval = 1.5f;
        public GameObject spawnSquare;

        GridManager gridManager = new();
        ShapeManager shapeManager = new();
        SquareManager squareManager = new();

        #region Lifecycle: Awake
        void Awake() {

            // ==== Instantiate ====
            ctx = new GameContext();

            assetModule = new AssetModule();

            userEntity = new UserEntity();
            squareRepository = new SquareRepository();

            // ==== Inject ====
            ctx.assetModule = assetModule;

            ctx.userEntity = userEntity;
            ctx.squareRepository = squareRepository;

            // ==== Pre Init ====
            StartCoroutine(PreInitIE());

        }

        IEnumerator PreInitIE() {
            yield return assetModule.LoadAllIE();
        }
        #endregion

        #region Lifecycle: Start 
        void Start() {
            GenerateShapeAndSquares();
            StartCoroutine(MoveSquareCoroutine());
        }
        #endregion

        #region Lifecycle: Update
        void Update() {
            if (Keyboard.current.aKey.wasPressedThisFrame) {
                SquareMove(MoveDirection.Left);
            }

            if (Keyboard.current.dKey.wasPressedThisFrame) {
                SquareMove(MoveDirection.Right);
            }

            if (Keyboard.current.wKey.wasPressedThisFrame) {
                SquareRotate(RotateDirection.Right);
            }

            if (Keyboard.current.sKey.wasPressedThisFrame) {
                SquareRotate(RotateDirection.Left);
            }

        }
        #endregion

        #region Lifecycle: OnDestroy
        void OnDestroy() {
#if UNITY_EDITOR
            assetModule.UnloadAll();
#endif
        }

        void OnApplicationQuit() {
            assetModule.UnloadAll();
        }
        #endregion

        void SquareRotate(RotateDirection direction) {
            SquareMoveStatus status = gridManager.IsEnableRotate(squaresOfCurrentShape, direction);
            if (status == SquareMoveStatus.EnableMove) {
                foreach (var square in squaresOfCurrentShape) {
                    squareManager.Roate(square, direction);
                }
                RenderCurrentSquare(squaresOfCurrentShape);
            }

            if (status == SquareMoveStatus.ReachBottom) {
                SquareSave();
            }

        }

        void SquareMove(MoveDirection moveDirection) {
            SquareMoveStatus status = gridManager.IsEnableMove(squaresOfCurrentShape, moveDirection);

            if (status == SquareMoveStatus.EnableMove) {
                foreach (var square in squaresOfCurrentShape) {
                    squareManager.Move(square, moveDirection);
                }
                RenderCurrentSquare(squaresOfCurrentShape);
            }

            if (status == SquareMoveStatus.ReachBottom) {
                SquareSave();
            }

        }

        void SquareSave() {
            gridManager.SaveSquare(squaresOfCurrentShape);
            RowFullCheck();
            squaresOfCurrentShape.Clear();
            GenerateShapeAndSquares();
        }

        System.Collections.IEnumerator MoveSquareCoroutine() {
            SquareMoveStatus status = gridManager.IsEnableMove(squaresOfCurrentShape, MoveDirection.Down);
            while (status == SquareMoveStatus.EnableMove) {
                yield return new WaitForSeconds(MoveInterval);
                SquareMove(MoveDirection.Down);
            }
        }

        void RowFullCheck() {
            HashSet<int> rows = new HashSet<int>();
            foreach (var square in squaresOfCurrentShape) {
                rows.Add(square.GridIndex.y);
            }

            List<int> shouldRemove = new();
            foreach (var row in rows) {
                if (!gridManager.IsRowFull(row)) {
                    shouldRemove.Add(row);
                }
            }

            foreach (var removeRow in shouldRemove) {
                rows.Remove(removeRow);
            }

            if (rows.Count > 0) {
                ClearRow(rows);
            }
        }

        void ClearRow(HashSet<int> rows) {
            // 销毁unity方块,播放特效
            foreach (var row in rows) {
                for (int x = 0; x < gridManager.grid.GetLength(0); x++) {
                    GameObject obj = gridManager.grid[x, row].SquareObj as GameObject;
                    if (obj) {
                        Destroy(obj);
                    }
                }
            }

            // 清除逻辑中的方块
            gridManager.ClearRow(rows);
            //重置场景中方块的位置
            for (int x = 0; x < gridManager.grid.GetLength(0); x++) {
                for (int y = rows.Min(); y < gridManager.grid.GetLength(1); y++) {
                    RenderSquare(gridManager.grid[x, y]);
                }
            }
        }


        void GenerateShapeAndSquares() {
            if (squaresOfHoldShape.Count > 0) {
                //hold方块的位置从等候区移动到游戏区
                squaresOfCurrentShape.AddRange(squaresOfHoldShape);
                squareManager.ChangeSquaresForShape(squaresOfCurrentShape, gridManager.gridTopCenter);

                //重新生成等候方块
                squaresOfHoldShape.Clear();
                squaresOfHoldShape = RandSpawnShapeAndRender(GridManager.Hold);
            } else {
                squaresOfCurrentShape = RandSpawnShapeAndRender();
            }
        }


        List<SquareEntity> RandSpawnShapeAndRender(string type = GridManager.Current) {
            //处理生成逻辑
            var shapeData = shapeManager.GetRandomShape();
            Vector2Int initGridIndex = type == GridManager.Current ? gridManager.gridTopCenter : gridManager.gridHoldCenter;
            List<SquareEntity> squares = squareManager.GenerateSquaresForShape(shapeData.squareIndex, initGridIndex);
            // 生成unity对象
            foreach (var square in squares) {
                Vector3 spawnPos = GridHelper.GetSquareWorldPos(square.GridIndex);
                Addressables.InstantiateAsync(square.AssetName, spawnPos, Quaternion.identity, spawnSquare.transform).Completed +=
                    (AsyncOperationHandle<GameObject> handle) => {
                        if (handle.Status == AsyncOperationStatus.Succeeded) {
                            square.SquareObj = handle.Result;
                        }
                    };
            }

            return squares;
        }

        void RenderCurrentSquare(List<SquareEntity> squares) {
            foreach (var square in squares) {
                RenderSquare(square);
            }
        }

        void RenderSquare(SquareEntity square) {
            GameObject squareObj = square.SquareObj as GameObject;
            if (squareObj) {
                squareObj.transform.position = GridHelper.GetSquareWorldPos(square.GridIndex);
            }
        }

    }
}