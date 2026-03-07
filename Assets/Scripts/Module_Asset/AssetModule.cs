using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PGC {

    public class AssetModule {

        Dictionary<ShapeType, ShapeSO> shapes;
        List<ShapeType> shapeTypes;
        AsyncOperationHandle shapeHandle;

        Dictionary<int, SquareSO> squares;
        List<int> squareTypeIDs;
        AsyncOperationHandle squareHandle;

        public AssetModule() {
            shapes = new Dictionary<ShapeType, ShapeSO>();
            shapeTypes = new List<ShapeType>();

            squares = new Dictionary<int, SquareSO>();
            squareTypeIDs = new List<int>();
        }

        public IEnumerator LoadAllIE() {
            yield return Shape_Load();
            yield return Square_Load();
        }

        public void UnloadAll() {
            if (shapeHandle.IsValid()) {
                Addressables.Release(shapeHandle);
            }
            if (squareHandle.IsValid()) {
                Addressables.Release(squareHandle);
            }
        }

        #region Shape
        IEnumerator Shape_Load() {
            string label = "Shape";
            AssetLabelReference labelReference = new AssetLabelReference();
            labelReference.labelString = label;
            // handle 是非托管内存, 不由GC管理
            var handle = Addressables.LoadAssetsAsync<ShapeSO>(labelReference, null);
            yield return handle;
            if (!handle.IsDone) {
                Debug.LogError($"Failed to load SquareSO with label {label}");
                yield break;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded) {
                Debug.LogError($"Failed to load SquareSO with label {label}");
                yield break;
            }

            IList<ShapeSO> list = handle.Result;
            foreach (var item in list) {
                bool succ = shapes.TryAdd(item.shapeType, item);
                if (!succ) {
                    Debug.LogError($"Failed to add SquareSO with shapeType {item.shapeType} to dictionary");
                }
                shapeTypes.Add(item.shapeType);
                Debug.Log($"Loaded SquareSO with shapeType {item.shapeType}");
            }

            shapeHandle = handle;
        }

        public bool Shape_TryGet(ShapeType shapeType, out ShapeSO shapeSO) {
            return shapes.TryGetValue(shapeType, out shapeSO);
        }

        public bool Shape_TryGetRandom(out ShapeSO shapeSO) {
            int index = UnityEngine.Random.Range(0, shapeTypes.Count);
            ShapeType shapeType = shapeTypes[index];
            return shapes.TryGetValue(shapeType, out shapeSO);
        }
        #endregion

        #region Square
        IEnumerator Square_Load() {
            string label = "Square";
            AssetLabelReference labelReference = new AssetLabelReference();
            labelReference.labelString = label;
            var handle = Addressables.LoadAssetsAsync<SquareSO>(labelReference, null);
            yield return handle;
            if (!handle.IsDone) {
                Debug.LogError($"Failed to load SquareSO with label {label}");
                yield break;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded) {
                Debug.LogError($"Failed to load SquareSO with label {label}");
                yield break;
            }

            IList<SquareSO> list = handle.Result;
            foreach (var item in list) {
                bool succ = squares.TryAdd(item.typeID, item);
                if (!succ) {
                    Debug.LogError($"Failed to add SquareSO with typeID {item.typeID} to dictionary");
                }
                squareTypeIDs.Add(item.typeID);
                Debug.Log($"Loaded SquareSO with typeID {item.typeID}");
            }

            squareHandle = handle;
        }

        public bool Square_TryGet(int typeID, out SquareSO squareSO) {
            return squares.TryGetValue(typeID, out squareSO);
        }

        public bool Square_TryGetRandom(out SquareSO squareSO) {
            int index = UnityEngine.Random.Range(0, squareTypeIDs.Count);
            int typeID = squareTypeIDs[index];
            return squares.TryGetValue(typeID, out squareSO);
        }
        #endregion
    }
}