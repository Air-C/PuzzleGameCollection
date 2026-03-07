using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PGC {

    public class AssetModule {

        Dictionary<ShapeType, SquareSO> squares;
        AsyncOperationHandle squareHandle;

        public AssetModule() {
            squares = new Dictionary<ShapeType, SquareSO>();
        }

        public IEnumerator LoadAllIE() {
            yield return Square_Load();
        }

        public void UnloadAll() {
            if (squareHandle.IsValid()) {
                Addressables.Release(squareHandle);
            }
        }

        #region Square
        IEnumerator Square_Load() {
            string label = "Square";
            AssetLabelReference labelReference = new AssetLabelReference();
            labelReference.labelString = label;
            // handle 是非托管内存, 不由GC管理
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
                bool succ = squares.TryAdd(item.shapeType, item);
                if (!succ) {
                    Debug.LogError($"Failed to add SquareSO with shapeType {item.shapeType} to dictionary");
                }
                Debug.Log($"Loaded SquareSO with shapeType {item.shapeType}");
            }

            squareHandle = handle;
        }

        public bool Square_TryGet(ShapeType shapeType, out SquareSO squareSO) {
            return squares.TryGetValue(shapeType, out squareSO);
        }
        #endregion
    }
}